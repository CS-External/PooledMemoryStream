using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Pools
{
    public interface IPoolStateProvider
    {
        int GetBlocksInUse();
        bool HasFreeBlocks();
    }
}
