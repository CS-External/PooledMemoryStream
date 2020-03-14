using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.Watchers.Default
{
    public class PoolWatcher: PoolWatcherBase
    {

        public event PoolWatcherHandleErrorEvent OnHandleError;
        public event PoolWatcherPoolStateChangedEvent OnPoolStateChanged;

        protected override void HandleError(StreamManagerPool p_Pool, Exception p_Exception)
        {
            PoolWatcherHandleErrorEvent l_HandleError = OnHandleError;

            if (l_HandleError == null)
                return;

            l_HandleError(this, new PoolWatcherHandleErrorEventArgs(p_Pool, p_Exception));
        }

        protected override void PoolStateChanged(StreamManagerPool p_Statistic, PoolWatcherPoolState p_OldState,
            PoolWatcherPoolState p_NewState)
        {
            PoolWatcherPoolStateChangedEvent l_Event = OnPoolStateChanged;

            if (l_Event == null)
                return;

            l_Event(this, new PoolWatcherPoolStateChangedEventArgs(p_Statistic, p_OldState, p_NewState));
        }
    }
}
