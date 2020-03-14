using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.Watchers.Default
{
    public class PoolWatcherPoolStatistic
    {
        private const int CONST_MAX_QUEUE_SIZE = 25;

        private Queue<long> m_BlocksInUseHistory;

        public PoolWatcherPoolStatistic(IPoolStateProvider p_Pool)
        {
            Pool = p_Pool;
            m_BlocksInUseHistory = new Queue<long>(CONST_MAX_QUEUE_SIZE);
            State = PoolWatcherPoolState.NothingChanged;
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
                if (m_BlocksInUseHistory.Count > CONST_MAX_QUEUE_SIZE)
                    m_BlocksInUseHistory.Peek();

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

                if (l_Diff > 0)
                {
                    // Growing
                    if (Pool.HasFreeBlocks())
                        State = PoolWatcherPoolState.Growing;
                    else
                        State = PoolWatcherPoolState.LimitExceededAndGrowing;

                }
                else
                if (l_Diff == 0)
                {
                    if (Pool.HasFreeBlocks())
                        State = PoolWatcherPoolState.NothingChanged;
                    else
                        State = PoolWatcherPoolState.LimitExceeded;
                }
                else
                {
                    // Shrinking
                    if (Pool.HasFreeBlocks())
                        State = PoolWatcherPoolState.Shrinking;
                    else
                        State = PoolWatcherPoolState.LimitExceededAndShrinking;
                }
            }


        }
    }
}
