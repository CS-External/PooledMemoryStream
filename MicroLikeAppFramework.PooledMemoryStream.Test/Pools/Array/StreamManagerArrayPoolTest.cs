using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.Array
{
    [TestClass]
    public class StreamManagerArrayPoolTest: StreamManagerPoolTestBase
    {
        [TestMethod]
        public void ReuseInstanceTest()
        {
            StreamManagerPool l_StreamManagerPool = CreatePool();
            MemoryBlock l_Block = l_StreamManagerPool.GetBlock();
            l_Block.ReturnBlock();

            MemoryBlock l_Block2 = l_StreamManagerPool.GetBlock();

            Assert.AreEqual(l_Block, l_Block2);
        }


        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerArrayPool("Test", 1, 1);
        }
    }
}
