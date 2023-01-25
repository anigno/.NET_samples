using System;
using System.IO;
using System.Threading;

namespace AnignoraIO.DirectoryWalking
{
    public class DirectoryWalker
    {
        #region Public Methods

        public void AbortWalk()
        {
            m_calculationThread.Abort();
            WalkingEnded(this, new DirectoryWalkerActivityEventArgs(""));
        }

        public void BeginWalk(string p_rootFolder, WalkingStyleEnum p_walkingStyle)
        {
            m_calculationThread = new Thread(walkThreadStart);
            m_calculationThread.IsBackground = true;
            StartCalculationThreadParams p = new StartCalculationThreadParams(p_rootFolder, p_walkingStyle);
            m_calculationThread.Start(p);
        }

        public void Walk(string p_rootFolder, WalkingStyleEnum p_walkingStyle)
        {
            WalkingStarted(this, new DirectoryWalkerActivityEventArgs(p_rootFolder));
            long sizeCount;
            int filesCount;
            walkRecurse(p_rootFolder, out sizeCount, out filesCount, p_walkingStyle);
            WalkingEnded(this, new DirectoryWalkerActivityEventArgs(p_rootFolder));
        }

        #endregion

        #region Events

        public event EventHandler<DirectoryWalkerActivityEventArgs> DirectoryDiscovered = delegate { };
        public event EventHandler<DirectoryDataEventArgs> DirectoryLeave = delegate { };
        public event EventHandler<DirectoryWalkerActivityEventArgs> FileDiscovered = delegate { };
        public event EventHandler<DirectoryWalkerActivityEventArgs> WalkingEnded = delegate { };
        public event EventHandler<DirectoryWalkerActivityEventArgs> WalkingStarted = delegate { };

        #endregion

        #region Private Methods

        private void walkRecurse(string p_rootFolder, out long p_sizeCount, out int p_filesCount, WalkingStyleEnum p_walkingStyle)
        {
            p_sizeCount = 0;
            p_filesCount = 0;
            if (!Directory.Exists(p_rootFolder)) return;
            DirectoryDiscovered(this, new DirectoryWalkerActivityEventArgs(p_rootFolder));
            DirectoryInfo DirectoryInfo = new DirectoryInfo(p_rootFolder);
            DirectoryInfo[] directories = DirectoryInfo.GetDirectoriesSafe();
            FileInfo[] files = DirectoryInfo.GetFilesSafe();
            if (p_walkingStyle == WalkingStyleEnum.DirectoriesFirst)
            {
                foreach (DirectoryInfo directory in directories)
                {
                    int subFiles;
                    long subSize;
                    walkRecurse(directory.FullName, out subSize, out subFiles, p_walkingStyle);
                    p_filesCount += subFiles;
                    p_sizeCount += subSize;
                }
            }
            foreach (FileInfo file in files)
            {
                p_filesCount++;
                p_sizeCount += file.Length;
                FileDiscovered(this, new DirectoryWalkerActivityEventArgs(file.FullName));
            }
            if (p_walkingStyle != WalkingStyleEnum.DirectoriesFirst)
            {
                foreach (DirectoryInfo directory in directories)
                {
                    walkRecurse(directory.FullName, out p_sizeCount, out p_filesCount, p_walkingStyle);
                }
            }
            DirectoryLeave(this, new DirectoryDataEventArgs(p_rootFolder, p_sizeCount, p_filesCount));
        }

        private void walkThreadStart(object p_params)
        {
            StartCalculationThreadParams p = (StartCalculationThreadParams) p_params;
            Walk(p.RootDirectory, p.WalkingStyle);
        }

        #endregion

        #region Fields

        private Thread m_calculationThread;

        #endregion
    }
}