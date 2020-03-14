using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.Watchers.Default
{
    public abstract class PoolWatcherBase : IPoolWatcher
    {
        protected readonly ConcurrentDictionary<StreamManagerPool, PoolWatcherPoolStatistic> m_PoolStatistics =
            new ConcurrentDictionary<StreamManagerPool, PoolWatcherPoolStatistic>();

        public virtual void Watch(List<StreamManagerPool> p_Pools)
        {
            foreach (StreamManagerPool l_Pool in p_Pools)
            {
                try
                {
                    PoolWatcherPoolStatistic l_Statistic =
                        m_PoolStatistics.GetOrAdd(l_Pool, p_Pool => new PoolWatcherPoolStatistic(p_Pool));
                    PoolWatcherPoolState l_OldState = l_Statistic.State;
                    l_Statistic.UpdateState();
                    PoolWatcherPoolState l_NewState = l_Statistic.State;

                    if (l_OldState == l_NewState)
                        continue;

                    PoolStateChanged(l_Pool, l_OldState, l_NewState);
                }
                catch (Exception e)
                {
                    HandleError(l_Pool, e);
                }
            }
        }

        protected abstract void HandleError(StreamManagerPool p_Pool, Exception p_Exception);

        protected abstract void PoolStateChanged(StreamManagerPool p_Statistic,
            PoolWatcherPoolState p_OldState, PoolWatcherPoolState p_NewState);
    }
}