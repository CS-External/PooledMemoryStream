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
            
            return new PooledMemoryStreamManager(new DefaultPoolChooserPolicy(m_Pools));
        }

        public static PooledMemoryStreamManagerBuilder Create()
        {
            return new PooledMemoryStreamManagerBuilder();
        }

        public static PooledMemoryStreamManager CreateMediumPool()
        {
            List<StreamManagerPool> l_List = new List<StreamManagerPool>();
            l_List.Add(new StreamManagerArrayPool("Small", 1024, 1000));
            l_List.Add(new StreamManagerArrayPool("Medium", 10 * 1024, 1000));
            l_List.Add(new StreamManagerArrayPool("Large", 30 * 1024, 1000));
            l_List.Add(new StreamManagerArrayPool("VeryLarge", 100 * 1024, 100));

            DefaultPoolChooserPolicy l_PoolChooser = new DefaultPoolChooserPolicy(l_List);
            return new PooledMemoryStreamManager(l_PoolChooser);
        }


        public static PooledMemoryStreamManager CreatePool(List<StreamManagerPool> p_List)
        {
            DefaultPoolChooserPolicy l_PoolChooser = new DefaultPoolChooserPolicy(p_List);
            return new PooledMemoryStreamManager(l_PoolChooser);
        }

    }
}
