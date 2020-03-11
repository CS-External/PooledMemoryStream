using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PooledMemoryStreams.Pools
{
    public abstract class StreamManagerPool
    {
        private int m_BlocksInUse = 0;
        private readonly int m_MaxBlocksInUseCount;
        private readonly string m_Name;

        public int MaxBlocksInUseCount
        {
            get { return m_MaxBlocksInUseCount; }
        }

        public String Name
        {
            get { return m_Name; }
        }

        protected StreamManagerPool(String p_Name)
        {
            m_Name = p_Name;
            m_MaxBlocksInUseCount = Int32.MaxValue;
        }

        protected StreamManagerPool(String p_Name, int p_MaxBlocksInUseCount)
        {
            m_Name = p_Name;
            m_MaxBlocksInUseCount = p_MaxBlocksInUseCount;
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
            return m_MaxBlocksInUseCount > m_BlocksInUse;
        }
    }
}
