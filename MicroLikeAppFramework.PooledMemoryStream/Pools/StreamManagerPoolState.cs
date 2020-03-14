namespace MicroLikeAppFramework.PooledMemoryStreams.Pools
{
    public enum StreamManagerPoolState
    {
        NothingSpecial,
        Growing,
        Shrinking,
        LimitReachedAndGrowing,
        LimitReached,
        LimitReachedAndShrinking
    }
}
