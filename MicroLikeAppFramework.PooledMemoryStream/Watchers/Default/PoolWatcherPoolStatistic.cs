using System;
using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;

namespace MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default
{
    public class PoolWatcherPoolStatistic
    {
        public const int CONST_MAX_QUEUE_SIZE = 50;
        private const double CONST_PERCENT_CHANGE_FACTOR = 0.10;

        private Queue<long> m_BlocksInUseHistory;

        public PoolWatcherPoolStatistic(IPoolStateProvider p_Pool)
        {
            Pool = p_Pool;
            m_BlocksInUseHistory = new Queue<long>(CONST_MAX_QUEUE_SIZE);
            State = new PoolWatcherPoolState(PoolWatcherPoolTrend.NothingChanged, false, 0);
        }

        public IPoolStateProvider Pool { get; private set; }
        public PoolWatcherPoolState State { get; private set; }
        

        public void UpdateState()
        {
            lock (this)
            {

                int l_BlocksInUse = Pool.GetBlocksInUse();
                m_BlocksInUseHistory.Enqueue(l_BlocksInUse);

                // If Limit is reached remote a point
                if (m_BlocksInUseHistory.Count < CONST_MAX_QUEUE_SIZE)
                {
                    // We Call Only Status when queue is full
                    RefreshLimitReached();
                    return;
                }

                int l_Count = m_BlocksInUseHistory.Count;

                if (l_Count <= 1)
                    return;

                int l_Pos = 0;
                Double l_First = 0;
                int l_FirstCount = 0;
                Double l_Second = 0;
                int l_SecondCount = 0;

                foreach (long l_DataPoint in m_BlocksInUseHistory)
                {
                    if (l_Pos < ((int)l_Count / 2))
                    {
                        l_First = l_First + l_DataPoint;
                        l_FirstCount++;
                    }
                    else
                    {
                        l_Second = l_Second + l_DataPoint;
                        l_SecondCount++;
                    }

                    l_Pos++;
                }


                Double l_FirstAvg = l_First / l_FirstCount;
                Double l_SecondAvg = l_Second / l_SecondCount;

                int l_Diff = (int)(l_SecondAvg - l_FirstAvg);

                Double l_MinDiff = Pool.MaxBlockCount * CONST_PERCENT_CHANGE_FACTOR;

                if (Math.Abs(l_Diff) < l_MinDiff)
                    l_Diff = 0;

                if (l_Diff > 0)
                {
                    State = new PoolWatcherPoolState(PoolWatcherPoolTrend.Growing, !Pool.HasFreeBlocks(), Pool.GetBlocksInUse());
                }
                else
                if (l_Diff == 0)
                {
                    State = new PoolWatcherPoolState(PoolWatcherPoolTrend.NothingChanged, !Pool.HasFreeBlocks(), Pool.GetBlocksInUse());
                }
                else
                {
                    State = new PoolWatcherPoolState(PoolWatcherPoolTrend.Shrinking, !Pool.HasFreeBlocks(), Pool.GetBlocksInUse());
                }
                

                m_BlocksInUseHistory.Clear();
            }


        }

        private void RefreshLimitReached()
        {
            bool l_HasFreeBlocks = Pool.HasFreeBlocks();
            
            if (State.LimitReached == !l_HasFreeBlocks)
                return;

            State = new PoolWatcherPoolState(State.Trend, !l_HasFreeBlocks, Pool.GetBlocksInUse());
        }
    }
}
