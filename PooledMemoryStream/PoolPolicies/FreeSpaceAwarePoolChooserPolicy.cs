using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public class FreeSpaceAwarePoolChooserPolicy: PoolChooserPolicyBase
    {
        public FreeSpaceAwarePoolChooserPolicy(List<StreamManagerPool> p_Pools) : base(p_Pools, null)
        {
        }

        public FreeSpaceAwarePoolChooserPolicy(List<PoolChooserPolicyPoolItem> p_Pools) : base(p_Pools, null)
        {
        }

        public FreeSpaceAwarePoolChooserPolicy(List<StreamManagerPool> p_Pools, StreamManagerPool p_FallbackPool) : base(p_Pools, p_FallbackPool)
        {
        }

        public FreeSpaceAwarePoolChooserPolicy(List<PoolChooserPolicyPoolItem> p_Pools, StreamManagerPool p_FallbackPool) : base(p_Pools, p_FallbackPool)
        {
        }

        protected override StreamManagerPool DoFindBestPool(long p_CurrentCapacity, long p_TargetCapacity)
        {
            Boolean l_CheckRange = true;
            StreamManagerPool l_LastPoolWithFreeBlocks = null;

            // To find the correct Buffersize we using the capacity
            // Simplified: Large Capacity -> Large Buffer

            foreach (PoolChooserPolicyPoolItem l_PoolItem in m_Pools)
            {
                if (l_CheckRange)
                {

                    if (l_PoolItem.IsInRange(p_TargetCapacity))
                    {
                        // Check if Space left in the Pool
                        if (l_PoolItem.Pool.HasFreeBlocks())
                            return l_PoolItem.Pool;

                        // Disable Range check for next checks
                        l_CheckRange = false;
                    }
                    else
                    {
                        if (l_PoolItem.Pool.HasFreeBlocks())
                            l_LastPoolWithFreeBlocks = l_PoolItem.Pool;
                    }
                }
                else
                {
                    if (l_PoolItem.Pool.HasFreeBlocks())
                        return l_PoolItem.Pool;
                }
            }

            //if we dont find a pool with matching size or large we return the last with some free space
            return l_LastPoolWithFreeBlocks;
        }

        protected override bool DoPoolHasFreeBlocks(StreamManagerPool p_Pool)
        {
            return p_Pool.HasFreeBlocks();
        }
    }
}
