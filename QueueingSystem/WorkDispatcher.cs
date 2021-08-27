using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using BaseLibS.Util;
using QueueingSystem.Drmaa;
using QueueingSystem.GenericCluster;
using QueueingSystem.Kubernetes;

namespace QueueingSystem{
	public abstract class WorkDispatcher{
		private const int initialDelay = 6;
		private const string clusterTypeDrmaa = "drmaa";
		private const string clusterTypeGeneric = "generic";
		private const string clusterTypeKubernetes = "kubernetes";
		private Thread[] workThreads;
		private Process[] externalProcesses;
		private string[] queuedJobIds;
		private Stack<int> toBeProcessed;
		public string InfoFolder{ get; }
		public bool DotNetCore{ get; }
		public bool ProfilePerformance{ get; }
		internal readonly int numInternalThreads;
		private readonly ISession session;
		public CalculationType CalculationType{ get; }

		public int MaxHeapSizeGb{ get; set; }

		public int Nthreads{ get; }

		public Func<int> NTasks{ get; }

		protected WorkDispatcher(int nThreads, int nTasks, string infoFolder, CalculationType calculationType,
			bool dotNetCore, bool profile) : this(nThreads, () => nTasks, infoFolder, calculationType, dotNetCore,
			profile, 1){ }

		protected WorkDispatcher(int nThreads, Func<int> nTasks, string infoFolder, CalculationType calculationType,
			bool dotNetCore, bool profile) : this(nThreads, nTasks, infoFolder, calculationType, dotNetCore, profile,
			1){ }

		protected WorkDispatcher(int nThreads, int nTasks, string infoFolder, CalculationType calculationType,
			bool dotNetCore, bool profile, int numInternalThreads) : this(nThreads, () => nTasks, infoFolder,
			calculationType, dotNetCore, profile, numInternalThreads){ }

		protected WorkDispatcher(int nThreads, Func<int> nTasks, string infoFolder, CalculationType calculationType,
			bool dotNetCore, bool profile, int numInternalThreads){
			Nthreads = nThreads;
			this.numInternalThreads = numInternalThreads;
			NTasks = nTasks;
			InfoFolder = infoFolder;
			DotNetCore = dotNetCore;
			ProfilePerformance = profile;
			if (!string.IsNullOrEmpty(infoFolder) && !Directory.Exists(infoFolder)){
				Directory.CreateDirectory(infoFolder);
			}
			CalculationType = calculationType;

			// TODO: remove in release
			if (Environment.GetEnvironmentVariable("MQ_CALC_TYPE") == "queue"){
				CalculationType = CalculationType.Queueing;
				session = GetSession();
				Console.WriteLine($"Using queueing session type: {session}");
			}
		}

		private int nTasksCached = -1;

		private int NTasksCached{
			get{
				if (nTasksCached == -1){
					nTasksCached = NTasks();
				}
				return nTasksCached;
			}
		}

		private static ISession GetSession(){
			var type = Environment.GetEnvironmentVariable("MQ_CLUSTER_TYPE") ?? clusterTypeDrmaa;
			switch (type){
				case clusterTypeDrmaa:{
					var s = DrmaaSession.GetInstance();
					s.Init();
					var nativeSpec = Environment.GetEnvironmentVariable("MQ_DRMAA_NATIVE_SPEC");
					if (nativeSpec != null){
						s.NativeSpecificationTemplate = nativeSpec;
					}
					return s;
				}
				case clusterTypeGeneric:{
					var submitCommand = Environment.GetEnvironmentVariable("MQ_CLUSTER_SUBMIT_CMD");
					var templatePath = Environment.GetEnvironmentVariable("MQ_CLUSTER_JOB_TEMPLATE_PATH");
					if (templatePath != null){
						return new GenericClusterSession(submitCommand, File.ReadAllText(templatePath));
					}
					return new GenericClusterSession(submitCommand);
				}
				case clusterTypeKubernetes:{
					var ns = Environment.GetEnvironmentVariable("MQ_KUBERNETES_NAMESPACE") ?? "default";
					var containerId = Environment.GetEnvironmentVariable("MQ_KUBERNETES_CONTAINER") ?? "mono";
					// TODO: config
					var volumes = Environment.GetEnvironmentVariable("MQ_KUBERNETES_VOLUMES") ?? "";
					var host = Environment.GetEnvironmentVariable("MQ_KUBERNETES_HOST");
					return new KubernetesSession(ns, containerId, volumes, host);
				}
				default:
					throw new Exception($"Unknown queueing system type: {type}");
			}
		}

		public int GetTotalThreads(){
			int res = 0;
			for (int i = 0; i < NTasks(); i++){
				res += GetNumInternalThreads(i);
			}
			return res;
		}

		public virtual int GetNumInternalThreads(int taskIndex){
			return numInternalThreads;
		}

		private bool DotNetCoreRunning => RuntimeInformation.FrameworkDescription.Contains("Core");

		public void Abort(){
			if (workThreads != null){
				foreach (Thread t in workThreads.Where(t => t != null)){
					if (DotNetCoreRunning) t.Interrupt();
					else t.Abort();
				}
			}
			if (CalculationType == CalculationType.ExternalProcess && externalProcesses != null){
				foreach (Process process in externalProcesses){
					if (process != null && Util.IsRunning(process)){
						try{
							process.Kill();
						} catch (Exception){ }
					}
				}
			}
			if (CalculationType == CalculationType.Queueing && queuedJobIds != null){
				foreach (string jobId in queuedJobIds){
					try{
						session.JobControl(jobId, Action.Terminate);
					} catch (QueuingSystemException ex){
						// TODO: handle DrmaaExceptions
						Console.Error.WriteLine(ex.ToString());
					}
				}
				// TODO: move Session Init/Exit code to upper level
//				Session.Exit();
			}
		}

		public void Start(){
			// TODO: remove in release, move Session.Init() to upper level  
//			if (CalculationType == CalculationType.Queueing)
//			{
//				_session.Init();
//			}
			toBeProcessed = new Stack<int>();
			for (int index = NTasksCached - 1; index >= 0; index--){
				toBeProcessed.Push(index);
			}
			workThreads = new Thread[Nthreads];
			externalProcesses = new Process[Nthreads];
			queuedJobIds = new string[Nthreads];
			for (int i = 0; i < Nthreads; i++){
				workThreads[i] = new Thread(Work){Name = "Thread " + i + " of " + GetType().Name};
				workThreads[i].Start(i);
				Thread.Sleep(initialDelay);
			}
			while (true){
				Thread.Sleep(1000);
				bool busy = false;
				for (int i = 0; i < Nthreads; i++){
					if (workThreads[i].IsAlive){
						busy = true;
						break;
					}
				}
				if (!busy){
					break;
				}
			}
			// TODO: move Session Init/Exit code to upper level
//			if (CalculationType == CalculationType.Queueing)
//			{
//				Session.Exit();	
//			}

			// TODO: waiting for fs sync
			// TODO: remove in release
			string sleepTime = Environment.GetEnvironmentVariable("MQ_WORK_SLEEP");
			if (sleepTime != null){
				Thread.Sleep(int.Parse(sleepTime));
			}
		}

		public string GetMessagePrefix(){
			return MessagePrefix + " ";
		}

		public abstract void Calculation(string[] args, Responder responder);
		public virtual bool IsFallbackPosition => true;

		protected virtual string GetComment(int taskIndex){
			return "";
		}

		protected virtual string Executable => "MaxQuantTask.exe";
		protected virtual string ExecutableCore => "MaxQuantTaskCore.dll";
		protected abstract object[] GetArguments(int taskIndex);
		protected abstract int Id{ get; }
		protected abstract string MessagePrefix{ get; }

		protected abstract int SoftwareId{ get; }

		private void Work(object threadIndex){
			while (toBeProcessed.Count > 0){
				int x;
				lock (this){
					if (toBeProcessed.Count > 0){
						x = toBeProcessed.Pop();
					} else{
						x = -1;
					}
				}
				if (x >= 0){
					DoWork(x, (int) threadIndex);
				}
			}
		}

		private void DoWork(int taskIndex, int threadIndex){
			switch (CalculationType){
				case CalculationType.ExternalProcess:
					ProcessSingleRunExternalProcess(taskIndex, threadIndex);
					break;
				case CalculationType.Thread:
					Calculation(GetStringArgs(taskIndex), new Responder());
					break;
				case CalculationType.Queueing:
					ProcessSingleRunQueueing(taskIndex, threadIndex, numInternalThreads);
					break;
			}
		}

		private IJobTemplate MakeJobTemplate(int taskIndex, int threadIndex, int numInternalThreads){
			IList<string> args = GetCommandLineArgs(taskIndex);
			string jobName = $"{GetFilename()}_{taskIndex}_{threadIndex}";
			string randSuffix = Guid.NewGuid().ToString();

			// TODO: Separate folder for job stdout/stderr?
			string outPath = Path.Combine(InfoFolder, $"{jobName}.{randSuffix}.out");
			// TODO: Separate folder for job stdout/stderr?
			string errPath = Path.Combine(InfoFolder, $"{jobName}.{randSuffix}.err");

			// Copying parent environment
			Dictionary<string, string> env = new Dictionary<string, string>();
			foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables()){
				env[entry.Key.ToString()] = entry.Value.ToString();
			}
			IJobTemplate jobTemplate = session.AllocateJobTemplate();
			jobTemplate.Arguments = args.ToArray();
			jobTemplate.OutputPath = outPath;
			jobTemplate.ErrorPath = errPath;
			jobTemplate.JobEnvironment = env;
			jobTemplate.Threads = GetNumInternalThreads(taskIndex);
			jobTemplate.JobName = jobName;
			jobTemplate.MaxMemorySize = MaxHeapSizeGb * 1024 * 1024 * 1024;
			return jobTemplate;
		}

		private void ProcessSingleRunQueueing(int taskIndex, int threadIndex, int numInternalThreads){
			IJobTemplate jobTemplate = MakeJobTemplate(taskIndex, threadIndex, numInternalThreads);

			// TODO: non atomic operation. When Aborted: job submitted, but queuedJobIds[threadIndex] not filled yet
			string jobId = session.Submit(jobTemplate);
			queuedJobIds[threadIndex] = jobId;

			// TODO: remove debug messages from future release
			Console.WriteLine($@"Created jobTemplate:
  parent command line args: {string.Join(", ", Environment.GetCommandLineArgs())}
  jobName:    {jobTemplate.JobName}
  args:       {string.Join(" ", jobTemplate.Arguments.Select(x => $"\"{x}\""))}
  outPath:    {jobTemplate.OutputPath}
  errPath:    {jobTemplate.ErrorPath}
  threads:    {jobTemplate.Threads}
  memory:     {jobTemplate.MaxMemorySize}
Submitted job {jobTemplate.JobName} with id: {jobId}
");
			try{
				var status = session.WaitForJobBlocking(jobId);
				if (status != Status.Success){
					Console.Error.WriteLine($"{jobTemplate.JobName}, jobId: {jobId}: \n" + jobTemplate.ReadStderr());
					throw new Exception(
						$"Exception during execution of external job: {jobTemplate.JobName}, jobId: {jobId}, status: {status}");
				} else{
					Console.WriteLine($"Job \"{jobTemplate.JobName}\" with id {jobId} finished successfully");
				}
			} finally{
				// TODO: Maybe introduce flag (cleanup or not, for debugging purposes)
				jobTemplate.Cleanup();
			}
		}

		internal IList<string> GetCommandLineArgs(int taskIndex){
			string cmd = GetCommandFilename();
			string[] logArgs = GetLogArgs(taskIndex, taskIndex);
			string[] calcArguments = GetStringArgs(taskIndex);
			List<string> result = new List<string>();
			if (Util.IsRunningOnMono()){
				result.Add("mono");
				// http://www.mono-project.com/docs/about-mono/releases/4.0.0/#floating-point-optimizations
				result.AddRange(new[]{"--optimize=all,float32", "--server", cmd});
				result.AddRange(logArgs);
				result.AddRange(calcArguments);
			} else if (DotNetCore){
				result.Add("dotnet");
				result.Add(cmd);
				result.AddRange(logArgs);
				result.AddRange(calcArguments);
			} else{
				result.Add(cmd);
				result.AddRange(logArgs);
				result.AddRange(calcArguments);
			}
			return result;
		}

		protected Process GetProcess(IList<string> args){
			bool isUnix = FileUtils.IsUnix();
			string cmd = args[0];
			string argsStr = string.Join(" ", Util.WrapArgs(args.Skip(1)));
			ProcessStartInfo psi = new ProcessStartInfo(cmd, argsStr);
			if (isUnix){
				psi.WorkingDirectory = Directory.GetDirectoryRoot(cmd);
				if (MaxHeapSizeGb > 0){
					psi.EnvironmentVariables["MONO_GC_PARAMS"] = "max-heap-size=" + MaxHeapSizeGb + "g";
				}
			} else{
				psi.WorkingDirectory = FileUtils.executablePath;
			}
			psi.EnvironmentVariables["PPID"] = Process.GetCurrentProcess().Id.ToString();
			psi.WindowStyle = ProcessWindowStyle.Hidden;
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;
			psi.RedirectStandardError = true;
			psi.RedirectStandardOutput = true;
			Process externalProcess = new Process{StartInfo = psi};
			externalProcess.OutputDataReceived += (sender, eventArgs) => { Console.WriteLine(eventArgs.Data); };
			externalProcess.ErrorDataReceived += (sender, eventArgs) => { Console.Error.WriteLine(eventArgs.Data); };
			return externalProcess;
		}

		private void ProcessSingleRunExternalProcess(int taskIndex, int threadIndex){
			IList<string> args = GetCommandLineArgs(taskIndex);
			Process externalProcess = GetProcess(args);
			externalProcesses[threadIndex] = externalProcess;
			externalProcesses[threadIndex].Start();
			int processid = externalProcesses[threadIndex].Id;
			externalProcesses[threadIndex].WaitForExit();
			string stdErr = externalProcess.StandardError.ReadToEnd();
			string stdOut = externalProcess.StandardOutput.ReadToEnd();
			int exitcode = externalProcesses[threadIndex].ExitCode;
			externalProcesses[threadIndex].Close();
			if (exitcode != 0){
				throw new Exception("Exception during execution of external process: " + processid + " " + stdErr);
			}
		}

		private string GetName(int taskIndex){
			return GetFilename() + " (" + Util.IntString(taskIndex + 1, NTasksCached) + "/" + NTasksCached + ")";
		}

		private string[] GetLogArgs(int taskIndex, int id){
			return new[]{
				InfoFolder, GetFilename(), id.ToString(), GetName(taskIndex), GetComment(taskIndex), "Process",
				$"\"{Id}\"", $"\"{SoftwareId}\""
			};
		}

		public string GetFilename(){
			return GetMessagePrefix().Trim().Replace("/", "").Replace("(", "_").Replace(")", "_").Replace(" ", "_");
		}

		public string GetCommandFilename(){
			return "\"" + FileUtils.executablePath + Path.DirectorySeparatorChar +
			       (DotNetCore ? ExecutableCore : Executable) + "\"";
		}

		private string[] GetStringArgs(int taskIndex){
			object[] o = GetArguments(taskIndex);
			string[] args = new string[o.Length];
			for (int i = 0; i < o.Length; i++){
				args[i] = $"{o[i]}";
			}
			return args;
		}
	}
}