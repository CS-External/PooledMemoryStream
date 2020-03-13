using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.Watchers
{
    public interface IPoolWatcher
    {
        void Watch(List<StreamManagerPool> p_Pools);
    }
}
