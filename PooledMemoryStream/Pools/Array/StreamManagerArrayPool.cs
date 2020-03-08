using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Pools.Array
{
    public class StreamManagerArrayPool: StreamManagerPool
    {
        private ConcurrentStack<ArrayMemoryBlock> m_Pool;
        private int m_BlockSize;
        public int MaxItemsInPool { get; private set; }

        public StreamManagerArrayPool(int p_BlockSize, int p_MaxItemsInPool) : this(p_BlockSize, p_MaxItemsInPool, Int32.MaxValue)
        {
            
        }

        public StreamManagerArrayPool(int p_BlockSize, int p_MaxItemsInPool, int p_MaxBlocksInUseCount) : base(p_MaxBlocksInUseCount)
        {
            MaxItemsInPool = p_MaxItemsInPool;
            m_BlockSize = p_BlockSize;
            m_Pool = new ConcurrentStack<ArrayMemoryBlock>();
        }

        protected override void DoReturnBlock(MemoryBlock p_Block)
        {
            ArrayMemoryBlock l_Block = (ArrayMemoryBlock)p_Block;

            // Check if there is some free Space in the Pool
            if (m_Pool.Count < MaxItemsInPool)
            {
                m_Pool.Push(l_Block);
            }
        }

        protected override MemoryBlock DoGetBlock()
        {
            ArrayMemoryBlock l_Block;

            if (m_Pool.TryPop(out l_Block))
            {
                return l_Block;
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
