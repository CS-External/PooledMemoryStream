using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;

namespace PooledMemoryStream.Test.Pools.Array
{
    public class StreamManagerArrayPoolTest: StreamManagerPoolTestBase
    {
        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerArrayPool(1, 1);
        }
    }
}
