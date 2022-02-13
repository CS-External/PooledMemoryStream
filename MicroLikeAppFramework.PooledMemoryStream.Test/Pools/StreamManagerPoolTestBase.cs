using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools
{
    public abstract class StreamManagerPoolTestBase
    {
        [TestMethod]
        public void GetAndReturnTest()
        {
            StreamManagerPool l_Pool = CreatePool();

            Assert.AreEqual(0, l_Pool.GetBlocksInUse());
            Assert.IsTrue(l_Pool.HasFreeBlocks());

            MemoryBlock l_Block = l_Pool.GetBlock();
            Assert.IsNotNull(l_Block);
            Assert.AreEqual(1, l_Pool.GetBlocksInUse());

            l_Block.ReturnBlock();
            Assert.AreEqual(0, l_Pool.GetBlocksInUse());

        }


        public abstract StreamManagerPool CreatePool();
    }
}
