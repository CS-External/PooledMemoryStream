using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;
using Xunit;

namespace PooledMemoryStream.Test.Pools.Array
{
    public class StreamManagerArrayPoolTest: StreamManagerPoolTestBase
    {
        [Fact]
        public void ReuseInstanceTest()
        {
            StreamManagerPool l_StreamManagerPool = CreatePool();
            MemoryBlock l_Block = l_StreamManagerPool.GetBlock();
            l_Block.ReturnBlock();

            MemoryBlock l_Block2 = l_StreamManagerPool.GetBlock();

            Assert.Equal(l_Block, l_Block2);
        }

        [Fact]
        public void OnPoolToSmallTest()
        {
            Boolean l_PoolToSmall = false;

            StreamManagerArrayPool l_StreamManagerPool = new StreamManagerArrayPool("Test", 1, 1);
            l_StreamManagerPool.OnPoolToSmall += p_Pool => { l_PoolToSmall = true; };

            Assert.False(l_PoolToSmall);
            MemoryBlock l_Block = l_StreamManagerPool.GetBlock();
            Assert.False(l_PoolToSmall);
            MemoryBlock l_Block2 = l_StreamManagerPool.GetBlock();
            Assert.True(l_PoolToSmall);
        }

        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerArrayPool("Test", 1, 1);
        }
    }
}
