using MicroLikeAppFramework.PooledMemoryStreams.Watchers;

namespace MicroLikeAppFramework.PooledMemoryStreams.Builders
{
    public interface IPoolWatcherBuilder: IPoolBuilder
    {
        IPoolBuilder UseTrigger<T>() where T : IPoolWatcherTrigger;
        IPoolBuilder UseTrigger(IPoolWatcherTrigger p_PoolWatcherTrigger);
    }
}
