using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Watchers;

namespace PooledMemoryStreams.Builders
{
    public interface IPoolWatcherBuilder: IPoolBuilder
    {
        IPoolBuilder UseTrigger<T>() where T : IPoolWatcherTrigger;
        IPoolBuilder UseTrigger(IPoolWatcherTrigger p_PoolWatcherTrigger);
    }
}
