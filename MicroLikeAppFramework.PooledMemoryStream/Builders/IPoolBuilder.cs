using System;
using MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;

namespace MicroLikeAppFramework.PooledMemoryStreams.Builders
{
    public interface IPoolBuilder: IPoolManagerBuilder
    {
        IPoolBuilder AddPool(StreamManagerArrayPool p_Pool);
        IPoolBuilder UseFallBackPool(StreamManagerArrayPool p_Pool);
        IPoolPolicyBuilder UsePolicy<T>() where T : IPoolChooserPolicy;
        IPoolPolicyBuilder UsePolicy(Type p_Type);
    }
}
