using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Builders
{
    public interface IPoolManagerBuilder
    {
        PooledMemoryStreamManager Build();
    }
}
