using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools.Array;

namespace PooledMemoryStreams.Builders
{
    public interface IPoolBuilder: IPoolManagerBuilder
    {
        IPoolBuilder AddPool(StreamManagerArrayPool p_Pool);
        IPoolBuilder UseFallBackPool(StreamManagerArrayPool p_Pool);
        IPoolPolicyBuilder UsePolicy<T>() where T : IPoolChooserPolicy;
        IPoolPolicyBuilder UsePolicy(Type p_Type);
    }
}
