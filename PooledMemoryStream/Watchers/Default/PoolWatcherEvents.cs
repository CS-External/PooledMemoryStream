using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.Watchers.Default
{
    public delegate void PoolWatcherHandleErrorEvent(PoolWatcher p_Pool, PoolWatcherHandleErrorEventArgs p_Args);

    public class PoolWatcherHandleErrorEventArgs : EventArgs
    {
        public PoolWatcherHandleErrorEventArgs(StreamManagerPool p_Pool, Exception p_Exception)
        {
            Pool = p_Pool;
            Exception = p_Exception;
        }

        public StreamManagerPool Pool { get; private set; }
        public Exception Exception { get; private set; }
        
    }

    public delegate void PoolWatcherPoolStateChangedEvent(PoolWatcher p_Pool, PoolWatcherPoolStateChangedEventArgs p_Args);

    public class PoolWatcherPoolStateChangedEventArgs : EventArgs
    {
        public PoolWatcherPoolStateChangedEventArgs(StreamManagerPool p_Pool, PoolWatcherPoolState p_OldState, PoolWatcherPoolState p_NewState)
        {
            Pool = p_Pool;
            OldState = p_OldState;
            NewState = p_NewState;
        }

        public StreamManagerPool Pool { get; private set; }
        public PoolWatcherPoolState OldState { get; private set; }
        public PoolWatcherPoolState NewState { get; private set; }
    }
}
