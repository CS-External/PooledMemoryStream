using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Watchers
{
    public interface IPoolWatcherTrigger
    {
        void Start(Action p_Callback);
        void Stop();
    }
}
