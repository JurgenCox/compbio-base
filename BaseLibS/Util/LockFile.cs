using System;
using System.IO;
using System.Threading;

namespace BaseLibS.Util{
	/// <summary>
	/// Class for creation and tracking of a lock-file, which can be used by different
	/// processes to synchronyze their actions on a single directory. 
	/// 
	/// <code>
	/// LockFile lock = new LockFile("d:\\test\\");
	/// try {
	///		// wait indefinite for the lock
	///		lock.Lock(-1);
	/// 
	///		// run functionality here
	/// } catch (Exception e) {
	///		;
	/// }
	/// 
	/// // release our lock when it exists.
	/// lock.Release();
	/// </code>
	/// </summary>
	public class LockFile{
		private FileStream handle;
		private readonly Random random;
		private readonly string lockFilePath;
		// constructor(s)
		/// <summary>
		/// Constructs a new instance of a lock-file in the given path. After this the actual lock-file will not
		/// have been created, which needs to be done with a call to <see cref="Lock"/>. When the directory does
		/// not exist it is created.
		/// </summary>
		/// <param name="path">The path where the lock-file is to be written.</param>
		public LockFile(string path, Random r = null){
			handle = null;
			lockFilePath = Path.Combine(path, ".lock");
		    random = r ?? new Random();
            Directory.CreateDirectory(path);
		}

        // implementation
        /// <summary>
        /// Creates the actual lock-file, securing exclusive usage of the required resources in a multi-process
        /// system. A maximum waiting time can be set to wait for gaining the lock on the file, which is set
        /// to infinity with the value -1 (ie the process waits indefinitely).
        /// </summary>
        /// <returns>True when the lock has succeeded, false otherwise.</returns>
        public void Lock(){
		    Thread.Sleep(random.Next(1000, 10000));
		    while (true) {
		        if (!File.Exists(lockFilePath)) {
		            try {
		                handle = AcquireFileStream(lockFilePath);
		                break;
		            } catch (Exception) {
		                handle = null;
		            }
		        }
		        Thread.Sleep(5000);
		    }
		}

	    private static FileStream AcquireFileStream(string lockPath) {
	        return new FileStream(
	            lockPath,
	            FileMode.Create,
	            FileAccess.ReadWrite,
	            FileShare.None,
	            bufferSize: 32,
	            options: FileOptions.None);
	    }

        /// <summary>
        /// Releases the lock-file so other processes can grab the resources.
        /// </summary>
        public void Release() {
		    handle.Close();
		    File.Delete(lockFilePath);
		    handle = null;
        }
	}
}