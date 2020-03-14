using MicroLikeAppFramework.PooledMemoryStreams.Watchers;

namespace MicroLikeAppFramework.PooledMemoryStreams.Builders
{
    public interface IPoolPolicyBuilder: IPoolManagerBuilder
    {
        IPoolWatcherBuilder UseWatcher<T>() where T : IPoolWatcher;
        IPoolWatcherBuilder UseWatcher(IPoolWatcher p_PoolWatcher);

        
    }
}
