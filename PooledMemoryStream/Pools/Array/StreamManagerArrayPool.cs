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

        public StreamManagerArrayPool(int p_BlockSize): this(Int32.MaxValue, p_BlockSize)
        {
            
        }

        public StreamManagerArrayPool(int p_MaxBlockCount, int p_BlockSize) : base(p_MaxBlockCount)
        {
            m_BlockSize = p_BlockSize;
            m_Pool = new ConcurrentStack<ArrayMemoryBlock>();
        }

        protected override void DoReturnBlock(MemoryBlock p_Block)
        {
            ArrayMemoryBlock l_Block = (ArrayMemoryBlock)p_Block;

            // Check if there is some free Space in the Pool
            if (m_Pool.Count < MaxBlockCount)
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
