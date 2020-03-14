using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;

namespace MicroLikeAppFramework.PooledMemoryStreams.Watchers
{
    public interface IPoolWatcher
    {
        void Watch(List<StreamManagerPool> p_Pools);
    }
}
