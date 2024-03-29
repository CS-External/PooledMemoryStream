﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.File
{
    [TestClass]
    public class FileMemoryBlockIWithOffsetTest : MemoryBlockTestBase, IDisposable
    {
        private List<String> m_FilesToDelete = new List<string>();

        protected override MemoryBlock DoCreateBlock(byte[] p_Content) 
        {
            string l_TempFileName = Path.GetTempFileName();
            m_FilesToDelete.Add(l_TempFileName);

            FileStream l_FileStream = new FileStream(l_TempFileName, FileMode.Open);
            l_FileStream.SetLength(p_Content.Length * 2);
            l_FileStream.Position = p_Content.Length;
            l_FileStream.Write(p_Content, 0, p_Content.Length);
            return new FileMemoryBlock(new StreamManagerFilePool("test", Path.GetTempPath(), 0), l_FileStream, p_Content.Length, null);
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
