using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DrmaaNet;
using Action = DrmaaNet.Action;


namespace BaseLibS.Util {
	public abstract class WorkDispatcher {
		private const int initialDelay = 6;
		private readonly int nTasks;
		private Thread[] workThreads;
		private Process[] externalProcesses;
		private string[] queuedJobIds;
		private Stack<int> toBeProcessed;
		private readonly string infoFolder;
		private readonly bool dotNetCore;
		private readonly int numInternalThreads;
		
		// TODO: remove in release
		private static bool sessionInited = false;

		protected WorkDispatcher(int nThreads, int nTasks, string infoFolder, CalculationType calculationType,
			bool dotNetCore) : this(nThreads, nTasks, infoFolder, calculationType, dotNetCore, 1)
		{
			// TODO: remove in release
			if (Environment.GetEnvironmentVariable("MQ_CALC_TYPE") == "queue")
			{
				CalculationType = CalculationType.Queueing;	
			}
			
		}

		protected WorkDispatcher(int nThreads, int nTasks, string infoFolder, CalculationType calculationType,
			bool dotNetCore, int numInternalThreads) {
			Nthreads = Math.Min(nThreads, nTasks);
			this.numInternalThreads = numInternalThreads;
			this.nTasks = nTasks;
			this.infoFolder = infoFolder;
			this.dotNetCore = dotNetCore;
			if (!string.IsNullOrEmpty(infoFolder) && !Directory.Exists(infoFolder)) {
				Directory.CreateDirectory(infoFolder);
			}
			CalculationType = calculationType;
		}

		public int MaxHeapSizeGb { get; set; } 

		public int Nthreads { get; }

		public void Abort() {
			if (workThreads != null) {
				foreach (Thread t in workThreads.Where(t => t != null)) {
					t.Abort();
				}
			}
			if (CalculationType == CalculationType.ExternalProcess && externalProcesses != null) {
				foreach (Process process in externalProcesses) {
					if (process != null && IsRunning(process)) {
						try {
							process.Kill();
						} catch (Exception) { }
					}
				}
			}

			if (CalculationType == CalculationType.Queueing && queuedJobIds != null)
			{
				foreach (string jobId in queuedJobIds)
				{
					try
					{
						Session.JobControl(jobId, Action.Terminate);
					}
					catch (DrmaaException ex)
					{
						Console.Error.WriteLine(ex.ToString());
					}
					
				}
			}
		}

		public static bool IsRunning(Process process) {
			if (process == null) return false;
			try {
				Process.GetProcessById(process.Id);
			} catch (Exception) {
				return false;
			}
			return true;
		}

		public void Start()
		{
			Console.WriteLine(
				$"type: {GetType()}, CalculationType: {CalculationType}, nThreads: {Nthreads}, nTasks: {nTasks}, numIntenalThreads: {numInternalThreads}");
			
			// TODO: remove in release, move Session.Init() to upper level  
			if (CalculationType == CalculationType.Queueing && !sessionInited)
			{
				Session.Init();
				sessionInited = true;
			}
			toBeProcessed = new Stack<int>();
			for (int index = nTasks - 1; index >= 0; index--) {
				toBeProcessed.Push(index);
			}
			workThreads = new Thread[Nthreads];
			externalProcesses = new Process[Nthreads];
			queuedJobIds = new string[Nthreads];
			
			for (int i = 0; i < Nthreads; i++) {
				workThreads[i] = new Thread(Work) {Name = "Thread " + i + " of " + GetType().Name};
				workThreads[i].Start(i);
				Thread.Sleep(initialDelay);
			}
			while (true) {
				Thread.Sleep(1000);
				bool busy = false;
				for (int i = 0; i < Nthreads; i++) {
					if (workThreads[i].IsAlive) {
						busy = true;
						break;
					}
				}
				if (!busy) {
					break;
				}
			}
		}

		public string GetMessagePrefix() {
			return MessagePrefix + " ";
		}

		public abstract void Calculation(string[] args, Responder responder);
		public virtual bool IsFallbackPosition => true;

		protected virtual string GetComment(int taskIndex) {
			return "";
		}

		protected abstract string Executable { get; }
		protected abstract string ExecutableCore { get; }
		protected abstract object[] GetArguments(int taskIndex);
		protected abstract int Id { get; }
		protected abstract string MessagePrefix { get; }

		private void Work(object threadIndex) {
			while (toBeProcessed.Count > 0) {
				int x;
				lock (this) {
					if (toBeProcessed.Count > 0) {
						x = toBeProcessed.Pop();
					} else {
						x = -1;
					}
				}
				if (x >= 0) {
					DoWork(x, (int) threadIndex);
				}
			}
		}

		private void DoWork(int taskIndex, int threadIndex) {
			switch (CalculationType) {
				case CalculationType.ExternalProcess:
					ProcessSingleRunExternalProcess(taskIndex, threadIndex);
					break;
				case CalculationType.Thread:
					Calculation(GetStringArgs(taskIndex), null);
					break;
				case CalculationType.Queueing:
					ProcessSingleRunQueueing(taskIndex, threadIndex, numInternalThreads);
					break;
			}
		}

		private JobTemplate MakeJobTemplate(int taskIndex, int threadIndex, int numInternalThreads)
		{
			string cmd = GetCommandFilename().Trim('"'); 
			
			// TODO: refactor to a function?
			List<string> args = new List<string>{"--optimize=all,float32", "--server", cmd};
			args.AddRange(GetLogArgs(taskIndex, taskIndex));
			args.Add(Id.ToString());
			args.AddRange(GetStringArgs(taskIndex));

			string jobName = $"{GetFilename()}_{taskIndex}_{threadIndex}";
			
			// TODO: Is it ok to get native spec (num of threads and other resources) via envvar?
			//			string nativeSpec = $" -l nodes=1:ppn={numInternalThreads}";
			// "-pe openmpi 40 -l h_rt=604800";
			string nativeSpec = (Environment.GetEnvironmentVariable("MQ_DRMAA") ?? "")
				.Replace("{threads}", numInternalThreads.ToString());
			
			string outPath = Path.Combine(infoFolder, $"{jobName}.out"); // TODO: Separate folder for job stdout/stderr?
			string errPath = Path.Combine(infoFolder, $"{jobName}.err"); // TODO: Separate folder for job stdout/stderr?\
			
			// TODO: refactor to a function?
			Dictionary<string, string> env = new Dictionary<string, string>();
			foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
			{
				env[entry.Key.ToString()] = entry.Value.ToString();
			}	
			
			JobTemplate jobTemplate = Session.AllocateJobTemplate();						
			jobTemplate.RemoteCommand = "mono"; // TODO: mono may be not in PATH 
			jobTemplate.Arguments = args.ToArray();
			jobTemplate.OutputPath = outPath;
			jobTemplate.ErrorPath = errPath;
			jobTemplate.JobEnvironment = env;
			jobTemplate.NativeSpecification = nativeSpec;
			jobTemplate.JobName = jobName;
			return jobTemplate;
		}
		
		private void ProcessSingleRunQueueing(int taskIndex, int threadIndex, int numInternalThreads)
		{
			JobTemplate jobTemplate = MakeJobTemplate(taskIndex, threadIndex, numInternalThreads);

			Console.WriteLine($"Submitting: cmd: {jobTemplate.RemoteCommand}");
			Console.WriteLine($"Submitting: jobName: {jobTemplate.JobName}");
			Console.WriteLine($"Submitting: args: {string.Join(" ", jobTemplate.Arguments.Select(x => $"\"{x}\""))}");
			Console.WriteLine($"Submitting: outPath: {jobTemplate.OutputPath}");
			Console.WriteLine($"Submitting: errPath: {jobTemplate.ErrorPath}");
			Console.WriteLine($"Submitting: nativeSpec: {jobTemplate.NativeSpecification}");
			
			// TODO: non atomic operation
			string jobId = jobTemplate.Submit();
			queuedJobIds[threadIndex] = jobId;
			
			Console.WriteLine($"Submitted job {jobTemplate.JobName} with id: {jobId}");
			var status = Session.WaitForJob(jobId);
			if (status != Status.Done)
			{
				if (File.Exists(jobTemplate.ErrorPath))
				{
					Console.Error.WriteLine(File.ReadAllText(jobTemplate.ErrorPath));		
				}
				throw new Exception($"Exception during execution of external job: {jobTemplate.JobName}, jobId: {jobId}, status: {status}");
			}
			if (File.Exists(jobTemplate.ErrorPath))
			{
				File.Delete(jobTemplate.ErrorPath);
			}
			
			if (File.Exists(jobTemplate.OutputPath))
			{
				File.Delete(jobTemplate.OutputPath);
			}
		}

		private void ProcessSingleRunExternalProcess(int taskIndex, int threadIndex) {
			bool isUnix = FileUtils.IsUnix();
			string cmd = GetCommandFilename();
			string args = GetLogArgsString(taskIndex, taskIndex) + GetCommandArguments(taskIndex);
			ProcessStartInfo psi = IsRunningOnMono() && !dotNetCore
				? new ProcessStartInfo("mono", " --optimize=all,float32 --server " + cmd + " " + args)
				: new ProcessStartInfo(cmd, args);
			if (isUnix) {
				psi.WorkingDirectory = Directory.GetDirectoryRoot(cmd);
				if (MaxHeapSizeGb > 0) {
					psi.EnvironmentVariables["MONO_GC_PARAMS"] = "max-heap-size=" + MaxHeapSizeGb + "g";
				}
			}
			Console.WriteLine($"Process run: {cmd} {args}");
			psi.WindowStyle = ProcessWindowStyle.Hidden;
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;
			psi.RedirectStandardError = true;
			psi.RedirectStandardOutput = true;
			var externalProcess = new Process {StartInfo = psi};
			externalProcess.OutputDataReceived += (sender, eventArgs) => { Console.WriteLine(eventArgs.Data); };
			externalProcess.ErrorDataReceived += (sender, eventArgs) => { Console.Error.WriteLine(eventArgs.Data); };
			externalProcesses[threadIndex] = externalProcess;
			externalProcesses[threadIndex].Start();
			int processid = externalProcesses[threadIndex].Id;
			externalProcesses[threadIndex].WaitForExit();
			int exitcode = externalProcesses[threadIndex].ExitCode;
			externalProcesses[threadIndex].Close();
			if (exitcode != 0) {
				throw new Exception("Exception during execution of external process: " + processid);
			}
		}

		/// <summary>
		/// http://www.mono-project.com/docs/gui/winforms/porting-winforms-applications/
		/// </summary>
		private static bool IsRunningOnMono() => Type.GetType("Mono.Runtime") != null;

		private string GetName(int taskIndex) {
			return GetFilename() + " (" + IntString(taskIndex + 1, nTasks) + "/" + nTasks + ")";
		}

		private static string IntString(int x, int n) {
			int npos = (int) Math.Ceiling(Math.Log10(n));
			string result = "" + x;
			if (result.Length >= npos) {
				return result;
			}
			return Repeat(npos - result.Length, "0") + result;
		}

		private static string Repeat(int n, string s) {
			StringBuilder b = new StringBuilder();
			for (int i = 0; i < n; i++) {
				b.Append(s);
			}
			return b.ToString();
		}

		private string[] GetLogArgs(int taskIndex, int id)
		{
			return new string[]
			{
				infoFolder, GetFilename(), taskIndex.ToString(), GetName(taskIndex), GetComment(taskIndex), "Process",
			};
		}
		
		private string GetLogArgsString(int taskIndex, int id)
		{
			return string.Join(" ", GetLogArgs(taskIndex, id).Select(x => $"\"{x}\""))+" ";
		}

		private string GetFilename() {
			return GetMessagePrefix().Trim().Replace("/", "").Replace("(", "_").Replace(")", "_").Replace(" ", "_");
		}

		private string GetCommandFilename() {
			return "\"" + FileUtils.executablePath + Path.DirectorySeparatorChar + (dotNetCore ? ExecutableCore : Executable) +
			       "\"";
		}

		private CalculationType CalculationType { get; }

		private string GetCommandArguments(int taskIndex) {
			object[] o = GetArguments(taskIndex);
			string[] args = new string[o.Length + 1];
			args[0] = $"\"{Id}\"";
			for (int i = 0; i < o.Length; i++) {
				object o1 = o[i];
				string s = Parser.ToString(o1);
				args[i + 1] = "\"" + s + "\"";
			}
			return StringUtils.Concat(" ", args);
		}

		private string[] GetStringArgs(int taskIndex) {
			object[] o = GetArguments(taskIndex);
			string[] args = new string[o.Length];
			for (int i = 0; i < o.Length; i++) {
				args[i] = $"{o[i]}";
			}
			return args;
		}
	}
}