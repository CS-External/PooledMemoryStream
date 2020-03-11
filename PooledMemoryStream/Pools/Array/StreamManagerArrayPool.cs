using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PooledMemoryStreams.Pools.Array
{
    public class StreamManagerArrayPool: StreamManagerPool
    {
        

        private int m_ItemsInPool = 0;
        private ConcurrentStack<ArrayMemoryBlock> m_Pool;
        private int m_BlockSize;
        private readonly int m_MaxItemsInPool;

        public int MaxItemsInPool
        {
            get { return m_MaxItemsInPool; }
        }

        public event StreamManagerArrayPoolEvent OnPoolToSmall; 

        public StreamManagerArrayPool(String p_Name, int p_BlockSize, int p_MaxItemsInPool) : this(p_Name, p_BlockSize, p_MaxItemsInPool, Int32.MaxValue)
        {
            
        }

        public StreamManagerArrayPool(String p_Name, int p_BlockSize, int p_MaxItemsInPool, int p_MaxBlocksInUseCount) : base(p_Name, p_MaxBlocksInUseCount)
        {
            m_MaxItemsInPool = p_MaxItemsInPool;
            m_BlockSize = p_BlockSize;
            m_Pool = new ConcurrentStack<ArrayMemoryBlock>();
        }

        protected override void DoReturnBlock(MemoryBlock p_Block)
        {
            ArrayMemoryBlock l_Block = (ArrayMemoryBlock)p_Block;

            // Check if there is some free Space in the Pool
            if (m_ItemsInPool < m_MaxItemsInPool)
            {
                Interlocked.Increment(ref m_ItemsInPool);
                m_Pool.Push(l_Block);
            }
        }

        protected override MemoryBlock DoGetBlock()
        {
            ArrayMemoryBlock l_Block;

            if (m_Pool.TryPop(out l_Block))
            {
                Interlocked.Decrement(ref m_ItemsInPool);
                return l_Block;
            }

            if (OnPoolToSmall != null)
            {

                if (GetBlocksInUse() + 1 > m_MaxItemsInPool)
                {
                    OnPoolToSmall(this);
                }

            }

            return new ArrayMemoryBlock(this, CreateByteBuffer(), m_BlockSize);

            
        }

        protected virtual byte[] CreateByteBuffer()
        {
            return new byte[m_BlockSize];
        }

        public override int GetBlockSize()
        {
            return m_BlockSize;
        }

        
    }
}
