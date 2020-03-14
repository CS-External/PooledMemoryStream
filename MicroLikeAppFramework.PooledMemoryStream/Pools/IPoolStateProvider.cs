namespace MicroLikeAppFramework.PooledMemoryStreams.Pools
{
    public interface IPoolStateProvider
    {
        int GetBlocksInUse();
        bool HasFreeBlocks();
        int MaxBlockCount { get; }
    }
}
