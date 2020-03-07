using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Pools
{
    public abstract class StreamManagerPool
    {

        protected StreamManagerPool(int p_BlockSize)
        {
            BlockSize = p_BlockSize;
        }

        public int BlockSize { get; private set; }
        public abstract MemoryBlock GetBlock();
        public abstract void ReturnBlock(MemoryBlock p_Block);

    }
}
