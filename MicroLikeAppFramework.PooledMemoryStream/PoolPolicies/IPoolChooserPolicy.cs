using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;

namespace MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies
{
    public interface IPoolChooserPolicy
    {
        StreamManagerPool FindBestPool(long p_CurrentCapacity, long p_TargetCapacity);
        List<StreamManagerPool> GetAllPools();
        bool PoolHasFreeBlocks(StreamManagerPool p_Pool);
    }
}
