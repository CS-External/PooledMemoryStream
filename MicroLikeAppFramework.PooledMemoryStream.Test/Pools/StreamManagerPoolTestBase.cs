using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using Xunit;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools
{
    public abstract class StreamManagerPoolTestBase
    {
        [Fact]
        public void GetAndReturnTest()
        {
            StreamManagerPool l_Pool = CreatePool();

            Assert.Equal(0, l_Pool.GetBlocksInUse());
            Assert.True(l_Pool.HasFreeBlocks());

            MemoryBlock l_Block = l_Pool.GetBlock();
            Assert.NotNull(l_Block);
            Assert.Equal(1, l_Pool.GetBlocksInUse());

            l_Block.ReturnBlock();
            Assert.Equal(0, l_Pool.GetBlocksInUse());

        }


        public abstract StreamManagerPool CreatePool();
    }
}
