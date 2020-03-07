using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PooledMemoryStreams.Pools.File
{
    public static class DirectoryUtils
    {
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

                    try
                    {
                        
                    }
                    catch (Exception p_Exception)
                    {
                        // Do nothing     
                    }
                    
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
