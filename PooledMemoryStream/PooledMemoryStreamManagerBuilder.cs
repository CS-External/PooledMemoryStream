using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;

namespace PooledMemoryStreams
{
    public class PooledMemoryStreamManagerBuilder
    {
        private List<StreamManagerPool> m_Pools;
        private StreamManagerArrayPoolEvent m_OnPoolToSmall;

        public PooledMemoryStreamManagerBuilder()
        {
            m_Pools = new List<StreamManagerPool>();
        }

        public PooledMemoryStreamManagerBuilder AddPool(StreamManagerArrayPool p_Pool)
        {
            m_Pools.Add(p_Pool);
            return this;
        }

        public PooledMemoryStreamManagerBuilder OnPoolToSmall(StreamManagerArrayPoolEvent p_Event)
        {
            m_OnPoolToSmall = p_Event;
            return this;
        }

        public PooledMemoryStreamManager Build()
        {
            foreach (StreamManagerPool l_Pool in m_Pools)
            {
                if (l_Pool is StreamManagerArrayPool)
                {
                    ((StreamManagerArrayPool) l_Pool).OnPoolToSmall += m_OnPoolToSmall;
                }
            } 
            
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
