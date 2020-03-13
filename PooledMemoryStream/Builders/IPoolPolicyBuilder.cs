using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Watchers;

namespace PooledMemoryStreams.Builders
{
    public interface IPoolPolicyBuilder: IPoolManagerBuilder
    {
        IPoolWatcherBuilder UseWatcher<T>() where T : IPoolWatcher;
        IPoolWatcherBuilder UseWatcher(IPoolWatcher p_PoolWatcher);

        
    }
}
