using System.IO;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.File;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.File
{
    public class StreamManagerFilePoolTest: StreamManagerPoolTestBase
    {
        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerFilePool("Test", Path.GetTempPath());
        }
    }
}
