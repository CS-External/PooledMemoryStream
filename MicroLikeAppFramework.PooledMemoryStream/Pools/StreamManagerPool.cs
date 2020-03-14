using System;
using System.Threading;

namespace MicroLikeAppFramework.PooledMemoryStreams.Pools
{
    public abstract class StreamManagerPool: IPoolStateProvider
    {
        private int m_BlocksInUse = 0;
        protected readonly int m_MaxBlockCount;
        protected readonly string m_Name;

        public int MaxBlockCount
        {
            get { return m_MaxBlockCount; }
        }

        public String Name
        {
            get { return m_Name; }
        }

        protected StreamManagerPool(String p_Name)
        {
            m_Name = p_Name;
            m_MaxBlockCount = Int32.MaxValue;
        }

        protected StreamManagerPool(String p_Name, int p_MaxBlockCount)
        {
            m_Name = p_Name;
            m_MaxBlockCount = p_MaxBlockCount;
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
            return m_MaxBlockCount > m_BlocksInUse;
        }
    }
}
