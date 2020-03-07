using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Pools.File
{
    public class StreamManagerFilePool: StreamManagerPool
    {
        private String m_CacheDirectory;

        public StreamManagerFilePool(string p_CacheDirectory) : base(Int32.MaxValue)
        {
            m_CacheDirectory = p_CacheDirectory;
            DirectoryUtils.EnsureCreated(m_CacheDirectory);
            DirectoryUtils.Cleanup(m_CacheDirectory);
        }

        

        public override MemoryBlock GetBlock()
        {
            
        }

        public override void ReturnBlock(MemoryBlock p_Block)
        {
            throw new NotImplementedException();
        }
    }
}
