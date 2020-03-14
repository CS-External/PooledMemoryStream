using System;

namespace MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default
{
    public class PoolWatcherPoolState
    {

        public PoolWatcherPoolState(PoolWatcherPoolTrend p_Trend, bool p_LimitReached, int p_BlocksInUse)
        {
            Trend = p_Trend;
            LimitReached = p_LimitReached;
            BlocksInUse = p_BlocksInUse;
        }

        public PoolWatcherPoolTrend Trend { get; private set; }
        public Boolean LimitReached { get; private set; }
        public int BlocksInUse { get; private set; }
        
    }
}
