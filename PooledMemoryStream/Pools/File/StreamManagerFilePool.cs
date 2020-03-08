using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PooledMemoryStreams.Pools.File
{
    public class StreamManagerFilePool: StreamManagerPool
    {
        private String m_CacheDirectory;

        public StreamManagerFilePool(string p_CacheDirectory): this(Int32.MaxValue, p_CacheDirectory)
        {
            
        }

        public StreamManagerFilePool(int p_MaxBlocksInUseCount, string p_CacheDirectory) : base(p_MaxBlocksInUseCount)
        {
            m_CacheDirectory = p_CacheDirectory;
            DirectoryUtils.EnsureCreated(m_CacheDirectory);
            DirectoryUtils.Cleanup(m_CacheDirectory);
        }

        public override int GetBlockSize()
        {
            return Int32.MaxValue;
        }

        
        protected override MemoryBlock DoGetBlock()
        {
            // Create new TempFile
            string l_FilePath = Path.Combine(m_CacheDirectory, Guid.NewGuid().ToString());
            FileStream l_Stream = new FileStream(l_FilePath, FileMode.CreateNew);
            return new FileMemoryBlock(this, l_Stream);
        }

        protected override void DoReturnBlock(MemoryBlock p_Block)
        {

            // Delete the TempFile
            FileMemoryBlock l_Block = (FileMemoryBlock)p_Block;
            FileStream l_Stream = (FileStream)l_Block.Stream;
            string l_Path = l_Stream.Name;
            l_Stream.Dispose();
            DirectoryUtils.SafeDelete(new FileInfo(l_Path));
        }
    }
}
