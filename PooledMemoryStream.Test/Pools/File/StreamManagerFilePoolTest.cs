using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.File;

namespace PooledMemoryStream.Test.Pools.File
{
    public class StreamManagerFilePoolTest: StreamManagerPoolTestBase
    {
        public override StreamManagerPool CreatePool()
        {
            return new StreamManagerFilePool(Path.GetTempPath());
        }
    }
}
