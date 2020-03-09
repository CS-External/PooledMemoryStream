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
        public static PooledMemoryStreamManager CreateMediumPool()
        {
            List<StreamManagerPool> l_List = new List<StreamManagerPool>();
            l_List.Add(new StreamManagerArrayPool(1024, 1000));
            l_List.Add(new StreamManagerArrayPool(10 * 1024, 1000));
            l_List.Add(new StreamManagerArrayPool(30 * 1024, 1000));
            l_List.Add(new StreamManagerArrayPool(100 * 1024, 100));

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
