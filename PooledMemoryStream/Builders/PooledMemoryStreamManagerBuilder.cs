﻿using System.Collections.Generic;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;

namespace PooledMemoryStreams.Builders
{
    public class PooledMemoryStreamManagerBuilder
    {
        private List<StreamManagerPool> m_Pools;

        public PooledMemoryStreamManagerBuilder()
        {
            m_Pools = new List<StreamManagerPool>();
        }

        public PooledMemoryStreamManagerBuilder AddPool(StreamManagerArrayPool p_Pool)
        {
            m_Pools.Add(p_Pool);
            return this;
        }

        public PooledMemoryStreamManager Build()
        {
            
            return new PooledMemoryStreamManager(new FreeSpaceAwarePoolChooserPolicy(m_Pools));
        }

        public static PooledMemoryStreamManagerBuilder Create()
        {
            return new PooledMemoryStreamManagerBuilder();
        }

        /*
         * Create a PoolBuilder for Large Application. The Pool Size is around  110 MB 
         */
        public static PooledMemoryStreamManagerBuilder CreateLargePoolBuilder()
        {
            return PooledMemoryStreamManagerBuilder.Create()
                .AddPool(new StreamManagerArrayPool("Small", 1024, 10000))
                .AddPool(new StreamManagerArrayPool("Medium", 10 * 1024, 5000))
                .AddPool(new StreamManagerArrayPool("Large", 30 * 1024, 2500))
                .AddPool(new StreamManagerArrayPool("VeryLarge", 100 * 1024, 100));
        }

        /*
         * Create a Pool for Large Application. The Pool Size is around  110 MB 
         */
        public static PooledMemoryStreamManager CreateLargePool()
        {
            return CreateLargePoolBuilder().Build();
        }


        public static PooledMemoryStreamManager CreatePool(List<StreamManagerPool> p_List)
        {
            FreeSpaceAwarePoolChooserPolicy l_PoolChooser = new FreeSpaceAwarePoolChooserPolicy(p_List);
            return new PooledMemoryStreamManager(l_PoolChooser);
        }

    }
}