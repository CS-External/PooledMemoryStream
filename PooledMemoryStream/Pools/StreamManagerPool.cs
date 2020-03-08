using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PooledMemoryStreams.Pools
{
    public abstract class StreamManagerPool
    {
        private int m_BlocksInUse = 0;

        public int MaxBlocksInUseCount { get; private set; }

        protected StreamManagerPool()
        {
            MaxBlocksInUseCount = Int32.MaxValue;
        }

        protected StreamManagerPool(int p_MaxBlocksInUseCount)
        {
            MaxBlocksInUseCount = p_MaxBlocksInUseCount;
        }

        protected abstract void DoReturnBlock(MemoryBlock p_Block);
        protected abstract MemoryBlock DoGetBlock();

        public abstract int GetBlockSize();

        public int GetBlocksInUse()
        {
            return m_BlocksInUse;
        }
        
        public MemoryBlock GetBlock()
        {
            MemoryBlock l_Block = DoGetBlock();
            Interlocked.Increment(ref m_BlocksInUse);
            return l_Block;
        }

        public void ReturnBlock(MemoryBlock p_Block)
        {
            Interlocked.Decrement(ref m_BlocksInUse);
            DoReturnBlock(p_Block);
        }

        public virtual bool HasFreeBlocks()
        {
            return MaxBlocksInUseCount > m_BlocksInUse;
        }
    }
}
