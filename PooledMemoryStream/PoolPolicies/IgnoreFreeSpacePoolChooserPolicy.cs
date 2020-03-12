using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public class IgnoreFreeSpacePoolChooserPolicy: PoolChooserPolicyBase
    {
        public IgnoreFreeSpacePoolChooserPolicy(List<StreamManagerPool> p_Pools, StreamManagerPool p_FallbackPool) : base(p_Pools, p_FallbackPool)
        {
        }

        public IgnoreFreeSpacePoolChooserPolicy(List<PoolChooserPolicyPoolItem> p_Pools, StreamManagerPool p_FallbackPool) : base(p_Pools, p_FallbackPool)
        {
        }

        public IgnoreFreeSpacePoolChooserPolicy(List<StreamManagerPool> p_Pools) : base(p_Pools, null)
        {
        }

        public IgnoreFreeSpacePoolChooserPolicy(List<PoolChooserPolicyPoolItem> p_Pools) : base(p_Pools, null)
        {
        }


        protected override StreamManagerPool DoFindBestPool(long p_CurrentCapacity, long p_TargetCapacity)
        {
            foreach (PoolChooserPolicyPoolItem l_PolicyPoolItem in m_Pools)
            {
                if (l_PolicyPoolItem.IsInRange(p_TargetCapacity))
                {
                    return l_PolicyPoolItem.Pool;
                }
            }

            return null;
        }

        protected override bool DoPoolHasFreeBlocks(StreamManagerPool p_Pool)
        {
            // This Policy ignore if the pool hast some free Blocks or not
            return true;
        }
    }
}
