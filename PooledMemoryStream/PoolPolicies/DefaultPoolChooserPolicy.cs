using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public class DefaultPoolChooserPolicy: PoolChooserPolicyBase
    {
        public DefaultPoolChooserPolicy(List<StreamManagerPool> p_Pools) : base(p_Pools)
        {
        }

        public DefaultPoolChooserPolicy(List<PoolChooserPolicyPoolItem> p_Pools) : base(p_Pools)
        {
        }

        public override StreamManagerPool FindBestPool(long p_CurrentCapacity, long p_TargetCapacity)
        {
            Boolean l_CheckRange = true;
            StreamManagerPool l_LastPoolWithFreeBlocks = null;

            foreach (PoolChooserPolicyPoolItem l_PoolItem in m_Pools)
            {
                if (l_CheckRange)
                {
                    if (l_PoolItem.IsInRange(p_TargetCapacity))
                    {
                        if (l_PoolItem.Pool.HasFreeBlocks())
                            return l_PoolItem.Pool;

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


            return l_LastPoolWithFreeBlocks;
        }
    }
}
