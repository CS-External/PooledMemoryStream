using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Watchers.Default
{
    public enum PoolWatcherPoolState
    {
        NothingChanged,
        Growing,
        Shrinking,
        LimitExceededAndGrowing,
        LimitExceededAndShrinking,
        LimitExceeded
    }
}
