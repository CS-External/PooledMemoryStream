using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Pools.Array
{
    public class StreamManagerArrayPool: StreamManagerPool
    {
        public StreamManagerArrayPool(int p_BlockSize) : base(p_BlockSize)
        {
        }

        public override MemoryBlock GetBlock()
        {
            throw new NotImplementedException();
        }

        public override void ReturnBlock(MemoryBlock p_Block)
        {
            throw new NotImplementedException();
        }
    }
}
