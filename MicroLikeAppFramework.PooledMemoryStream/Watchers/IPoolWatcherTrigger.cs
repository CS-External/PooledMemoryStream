using System;

namespace MicroLikeAppFramework.PooledMemoryStreams.Watchers
{
    public interface IPoolWatcherTrigger
    {
        void Start(Action p_Callback);
        void Stop();
    }
}
