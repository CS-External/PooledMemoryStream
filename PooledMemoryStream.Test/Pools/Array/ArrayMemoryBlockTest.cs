using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;
using Xunit;

namespace PooledMemoryStream.Test.Pools.Array
{
    public class ArrayMemoryBlockTest: MemoryBlockTestBase
    {
        protected override MemoryBlock DoCreateBlock(byte[] p_Content)
        {
            return new ArrayMemoryBlock(new StreamManagerArrayPool("test", 0, 0), p_Content, p_Content.Length);
        }
    }
}
