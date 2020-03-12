using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public interface IPoolChooserPolicy
    {
        StreamManagerPool FindBestPool(long p_CurrentCapacity, long p_TargetCapacity);
        List<StreamManagerPool> GetAllPools();
        bool PoolHasFreeBlocks(StreamManagerPool p_Pool);
    }
}
