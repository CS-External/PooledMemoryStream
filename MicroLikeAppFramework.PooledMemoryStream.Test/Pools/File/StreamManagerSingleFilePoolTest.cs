using System.IO;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.File
{
    [TestClass]
    public class StreamManagerSingleFilePoolTest: StreamManagerPoolTestBase
    {
        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerFilePool("Test", Path.GetTempPath(), 1);
        }
    }
}
