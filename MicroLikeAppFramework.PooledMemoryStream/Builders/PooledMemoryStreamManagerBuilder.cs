using System;
using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using MicroLikeAppFramework.PooledMemoryStreams.Watchers;
using MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default;

namespace MicroLikeAppFramework.PooledMemoryStreams.Builders
{
    public class PooledMemoryStreamManagerBuilder: IPoolBuilder, IPoolManagerBuilder, IPoolPolicyBuilder, IPoolWatcherBuilder
    {
        private List<StreamManagerPool> m_Pools;
        private StreamManagerPool m_FallbackPool;
        private IPoolChooserPolicy m_PoolChooserPolicy;
        private IPoolWatcher m_PoolWatcher;
        private IPoolWatcherTrigger m_PoolWatcherTrigger;
        
        public PooledMemoryStreamManagerBuilder()
        {
            m_Pools = new List<StreamManagerPool>();
        }

        public IPoolBuilder AddPool(StreamManagerPool p_Pool)
        {
            m_Pools.Add(p_Pool);
            return this;
        }

        public IPoolBuilder UseFallBackPool(StreamManagerPool p_Pool)
        {
            m_FallbackPool = p_Pool;
            return this;
        }

        public IPoolPolicyBuilder UsePolicy<T>() where T : IPoolChooserPolicy
        {
            return UsePolicy(typeof(T));
        }

        public IPoolPolicyBuilder UsePolicy(Type p_Type)
        {
            m_PoolChooserPolicy = (IPoolChooserPolicy)Activator.CreateInstance(p_Type, new object[] {m_Pools, m_FallbackPool});
            return this;
        }

        public PooledMemoryStreamManager Build()
        {
            if (m_PoolChooserPolicy == null)
            {
                UsePolicy<IgnoreFreeSpacePoolChooserPolicy>();
            }

            if (m_PoolWatcher != null && m_PoolWatcherTrigger == null)
            {
                UseTrigger<PoolWatcherTrigger>();
            }

            return new PooledMemoryStreamManager(m_PoolChooserPolicy, m_PoolWatcher, m_PoolWatcherTrigger);
        }

        public static PooledMemoryStreamManagerBuilder Create()
        {
            return new PooledMemoryStreamManagerBuilder();
        }

        /*
         * Create a PoolBuilder for Large Application. The Pool Size is around  110 MB 
         */
        public static IPoolBuilder CreateLargePoolBuilder()
        {
            return PooledMemoryStreamManagerBuilder.Create()
                .AddPool(new StreamManagerArrayPool("Small", 1024, 10000))
                .AddPool(new StreamManagerArrayPool("Medium", 10 * 1024, 5000))
                .AddPool(new StreamManagerArrayPool("Large", 30 * 1024, 2500))
                .AddPool(new StreamManagerArrayPool("VeryLarge", 100 * 1024, 100));
        }

        /*
         * Create a PoolBuilder for Medium Application. The Pool Size is around  60 MB 
         */
        public static IPoolBuilder CreateMediumPoolBuilder()
        {
            return PooledMemoryStreamManagerBuilder.Create()
                .AddPool(new StreamManagerArrayPool("Small", 1024, 5000))
                .AddPool(new StreamManagerArrayPool("Medium", 10 * 1024, 2500))
                .AddPool(new StreamManagerArrayPool("Large", 30 * 1024, 1250))
                .AddPool(new StreamManagerArrayPool("VeryLarge", 100 * 1024, 50));
        }

        /*
         * Create a PoolBuilder for Small Application. The Pool Size is around  30 MB 
         */
        public static IPoolBuilder CreateSmallPoolBuilder()
        {
            return PooledMemoryStreamManagerBuilder.Create()
                .AddPool(new StreamManagerArrayPool("Small", 1024, 2500))
                .AddPool(new StreamManagerArrayPool("Medium", 10 * 1024, 1250))
                .AddPool(new StreamManagerArrayPool("Large", 30 * 1024, 625))
                .AddPool(new StreamManagerArrayPool("VeryLarge", 100 * 1024, 25));
        }

        /*
         * Create a Pool for Large Application. The Pool Size is around  110 MB 
         */
        public static PooledMemoryStreamManager CreateLargePool()
        {
            return CreateLargePoolBuilder().Build();
        }

        /*
         * Create a Pool for Medium Application. The Pool Size is around  60 MB 
         */
        public static PooledMemoryStreamManager CreateMediumPool()
        {
            return CreateMediumPoolBuilder().Build();
        }

        /*
         * Create a Pool for Medium Application. The Pool Size is around  30 MB 
         */
        public static PooledMemoryStreamManager CreateSmallPool()
        {
            return CreateSmallPoolBuilder().Build();
        }

        public IPoolWatcherBuilder UseWatcher<T>() where T : IPoolWatcher
        {
            m_PoolWatcher = Activator.CreateInstance<T>();
            return this;
        }

        public IPoolWatcherBuilder UseWatcher(IPoolWatcher p_PoolWatcher)
        {
            m_PoolWatcher = p_PoolWatcher;
            return this;
        }

        public IPoolBuilder UseTrigger<T>() where T : IPoolWatcherTrigger
        {
            m_PoolWatcherTrigger = Activator.CreateInstance<T>();
            return this;
        }

        public IPoolBuilder UseTrigger(IPoolWatcherTrigger p_PoolWatcherTrigger)
        {
            m_PoolWatcherTrigger = p_PoolWatcherTrigger;
            return this;
        }
    }
}
