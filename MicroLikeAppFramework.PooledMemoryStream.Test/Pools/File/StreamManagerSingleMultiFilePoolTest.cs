using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.File;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.File
{
    public class StreamManagerSingleMultiFilePoolTest : StreamManagerPoolTestBase
    {
        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerFilePool("Test", Path.GetTempPath(), 2);
        }
    }
}
