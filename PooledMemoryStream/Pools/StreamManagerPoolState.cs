using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Pools
{
    public enum StreamManagerPoolState
    {
        NothingSpecial,
        Growing,
        Shrinking,
        LimitReachedAndGrowing,
        LimitReached,
        LimitReachedAndShrinking
    }
}
