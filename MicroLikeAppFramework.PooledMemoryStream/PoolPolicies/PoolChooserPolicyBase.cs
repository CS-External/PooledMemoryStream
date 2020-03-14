using System;
using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies.Common;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;

namespace MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies
{
    public abstract class PoolChooserPolicyBase : IPoolChooserPolicy
    {
        protected readonly StreamManagerPool m_FallbackPool;
        protected List<PoolChooserPolicyPoolItem> m_Pools;


        protected PoolChooserPolicyBase(List<StreamManagerPool> p_Pools, StreamManagerPool p_FallbackPool)
        {
            m_FallbackPool = p_FallbackPool;
            m_Pools = CreatePools(p_Pools);
        }

        protected PoolChooserPolicyBase(List<PoolChooserPolicyPoolItem> p_Pools, StreamManagerPool p_FallbackPool)
        {
            if (p_Pools.Count == 0)
                throw new ArgumentException("Pool without Items is not supported", nameof(p_Pools));

            m_FallbackPool = p_FallbackPool;
            m_Pools = p_Pools;
        }

        private List<PoolChooserPolicyPoolItem> CreatePools(List<StreamManagerPool> p_Pools)
        {
            if (p_Pools.Count == 0)
                throw new ArgumentException("Pool without Items is not supported", nameof(p_Pools));

            StreamManagerPoolIntervalCalculator l_Calculator = new StreamManagerPoolIntervalCalculator();
            return l_Calculator.CalcIntervals(p_Pools);
        }


        public StreamManagerPool FindBestPool(long p_CurrentCapacity, long p_TargetCapacity)
        {
            StreamManagerPool l_Pool = DoFindBestPool(p_CurrentCapacity, p_TargetCapacity);

            if (l_Pool == null)
            {
                return m_FallbackPool;
            }

            return l_Pool;
        }

        protected abstract StreamManagerPool DoFindBestPool(long p_CurrentCapacity, long p_TargetCapacity);

        public List<StreamManagerPool> GetAllPools()
        {
            List<StreamManagerPool> l_Pools = new List<StreamManagerPool>();

            foreach (PoolChooserPolicyPoolItem l_PoolItem in m_Pools)
            {
                l_Pools.Add(l_PoolItem.Pool);
            }

            return l_Pools;
        }

        public bool PoolHasFreeBlocks(StreamManagerPool p_Pool)
        {
            // Fallback Pool has always free space 
            if (p_Pool == m_FallbackPool)
                return true;

            return DoPoolHasFreeBlocks(p_Pool);
        }

        protected abstract bool DoPoolHasFreeBlocks(StreamManagerPool p_Pool);
    }

    
}
