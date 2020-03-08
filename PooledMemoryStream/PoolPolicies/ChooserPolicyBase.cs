using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public abstract class ChooserPolicyBase : IPoolChooserPolicy
    {
        private List<ChooserPolicyBaseItem> m_Pools;

        protected ChooserPolicyBase(List<StreamManagerPool> p_Pools)
        {
            m_Pools = CreatePools(p_Pools);
        }

        private List<ChooserPolicyBaseItem> CreatePools(List<StreamManagerPool> p_Pools)
        {
            p_Pools.Sort(SortByBlockSize);
            List<ChooserPolicyBaseItem> l_List = new List<ChooserPolicyBaseItem>();
            ChooserPolicyBaseItem l_Last = null;

            foreach (StreamManagerPool l_Pool in p_Pools)
            {
                ChooserPolicyBaseItem l_CurrentItem = new ChooserPolicyBaseItem();
                l_CurrentItem.Pool = l_Pool;

                if (l_Last != null)
                {
                    
                }
                else
                {
                    
                }



            }

            if (l_Last != null)
            {
                l_Last.End = Int64.MaxValue;
            }

            return l_List;
        }

        private int SortByBlockSize(StreamManagerPool p_X, StreamManagerPool p_Y)
        {
            return p_X.GetBlockSize().CompareTo(p_X.GetBlockSize());
        }

        public abstract StreamManagerPool FindBestPool(long p_CurrentCapacity, long p_TargetCapacity);

    }

    public class ChooserPolicyBaseItem
    {
        public long Start { get; set; }
        public long End { get; set; }
        public StreamManagerPool Pool { get; set; }
    }
}
