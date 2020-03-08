using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.PoolPolicies.Common;
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

            StreamManagerPoolIntervalCalculator l_Calculator = new StreamManagerPoolIntervalCalculator();
            return l_Calculator.CalcIntervals(p_Pools);
        }


        public abstract StreamManagerPool FindBestPool(long p_CurrentCapacity, long p_TargetCapacity);

    }

    
}
