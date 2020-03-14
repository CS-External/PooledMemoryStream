using System;
using System.Collections.Generic;
using System.IO;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.File;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.File
{
    public class FileMemoryBlockTest: MemoryBlockTestBase, IDisposable
    {
        private List<String> m_FilesToDelete = new List<string>();

        protected override MemoryBlock DoCreateBlock(byte[] p_Content)
        {
            string l_TempFileName = Path.GetTempFileName();
            m_FilesToDelete.Add(l_TempFileName);

            FileStream l_FileStream = new FileStream(l_TempFileName, FileMode.Open);
            l_FileStream.Write(p_Content, 0, p_Content.Length);    
            return new FileMemoryBlock(new StreamManagerFilePool("test", Path.GetTempPath(), 0), l_FileStream, 0, null);
        }

        public void Dispose()
        {
            foreach (string l_File in m_FilesToDelete)
            {
                DirectoryUtils.SafeDelete(new FileInfo(l_File));
            }

        }
    }
}
