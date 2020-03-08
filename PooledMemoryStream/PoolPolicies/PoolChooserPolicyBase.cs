using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public abstract class PoolChooserPolicyBase : IPoolChooserPolicy
    {
        protected List<PoolChooserPolicyPoolItem> m_Pools;

        protected PoolChooserPolicyBase(List<StreamManagerPool> p_Pools)
        {
            m_Pools = CreatePools(p_Pools);
        }

        protected PoolChooserPolicyBase(List<PoolChooserPolicyPoolItem> p_Pools)
        {
            if (p_Pools.Count == 0)
                throw new ArgumentException("Pool without Items is not supported", nameof(p_Pools));

            m_Pools = p_Pools;
        }

        private List<PoolChooserPolicyPoolItem> CreatePools(List<StreamManagerPool> p_Pools)
        {
            if (p_Pools.Count == 0)
                throw new ArgumentException("Pool without Items is not supported", nameof(p_Pools));

            // Sort by BlockSize
            p_Pools.Sort(SortByBlockSize);
            List<PoolChooserPolicyPoolItem> l_List = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Last = null;

            foreach (StreamManagerPool l_Pool in p_Pools)
            {
                PoolChooserPolicyPoolItem l_CurrentItem = new PoolChooserPolicyPoolItem();
                l_CurrentItem.Pool = l_Pool;

                if (l_Last == null)
                {
                    // First Item
                    l_CurrentItem.Start = 0;
                    continue;
                }

                int l_SpaceBetween = l_CurrentItem.Pool.GetBlockSize() - l_Last.Pool.GetBlockSize();

                if ()

                l_Last = l_CurrentItem;
            }

            l_Last.End = Int64.MaxValue;

            return l_List;
        }

        private int SortByBlockSize(StreamManagerPool p_X, StreamManagerPool p_Y)
        {
            return p_X.GetBlockSize().CompareTo(p_X.GetBlockSize());
        }

        public abstract StreamManagerPool FindBestPool(long p_CurrentCapacity, long p_TargetCapacity);

    }

    
}
