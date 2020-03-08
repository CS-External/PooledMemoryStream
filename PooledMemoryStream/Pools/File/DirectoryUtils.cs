using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PooledMemoryStreams.Pools.File
{
    public static class DirectoryUtils
    {
        private static int CONST_DELETE_RETRY_COUNT = 3;
        private static Object m_Lock = new object();

        public static void Cleanup(string p_CacheDirectory)
        {
            lock (m_Lock)
            {
                System.IO.DirectoryInfo l_Directory = new DirectoryInfo(p_CacheDirectory);

                if (!l_Directory.Exists)
                    return;

                foreach (FileInfo l_File in l_Directory.GetFiles())
                {
                    SafeDelete(l_File);
                }
            }

            
        }

        public static void SafeDelete(FileInfo p_File)
        {
            for (int i = 0; i < CONST_DELETE_RETRY_COUNT; i++)
            {
                try
                {
                    p_File.Delete();
                    break;
                }
                catch (Exception)
                {
                    // Do nothing     
                    Task.Delay(25).Wait();
                }
            }
        }

        public static void EnsureCreated(string p_CacheDirectory)
        {
            lock (m_Lock)
            {
                if (System.IO.Directory.Exists(p_CacheDirectory))
                    return;

                System.IO.Directory.CreateDirectory(p_CacheDirectory);
            }
        }

    }
}
