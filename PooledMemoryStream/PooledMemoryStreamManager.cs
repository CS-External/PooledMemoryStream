using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Watchers;

namespace PooledMemoryStreams
{
    public class PooledMemoryStreamManager: IDisposable
    {
        private IPoolChooserPolicy m_ChooserPolicy;
        private IPoolWatcher m_PoolWatcher;
        private IPoolWatcherTrigger m_PoolWatcherTrigger;

        public PooledMemoryStreamManager(IPoolChooserPolicy p_ChooserPolicy, IPoolWatcher p_PoolWatcher, IPoolWatcherTrigger p_PoolWatcherTrigger)
        {
            m_ChooserPolicy = p_ChooserPolicy;
            m_PoolWatcher = p_PoolWatcher;
            m_PoolWatcherTrigger = p_PoolWatcherTrigger;

            Init();
        }

        private void Init()
        {
            if (m_PoolWatcher == null && m_PoolWatcherTrigger == null)
                return;

            if (m_PoolWatcher != null && m_PoolWatcherTrigger == null)
                throw new ArgumentException("If you provide a PoolWatcher you need also provide also a PoolWatcherTrigger");

            m_PoolWatcherTrigger.Start(ExecuteWatcher);
        }

        public StreamManagerPool GetPoolByName(String p_Name)
        {
            return m_ChooserPolicy.GetAllPools().FirstOrDefault(x => x.Name == p_Name);
        }

        public List<StreamManagerPool> GetAllPools()
        {
            return m_ChooserPolicy.GetAllPools();
        }

        

        public Stream GetStream(long p_Capacity)
        {
            return new PooledMemoryStream(p_Capacity, this);
        }

        public Stream GetStream()
        {
            return new PooledMemoryStream(0, this);
        }

        public void Dispose()
        {
            if (m_PoolWatcherTrigger != null)
            {
                m_PoolWatcherTrigger.Stop();
            }
        }

        protected virtual void ExecuteWatcher()
        {
            try
            {
                List<StreamManagerPool> l_Pools = m_ChooserPolicy.GetAllPools();
                m_PoolWatcher.Watch(l_Pools);
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                    Debug.WriteLine("Error while execute watcher: " + e);
            }
        }

        protected internal virtual List<MemoryBlock> GetBlock(long p_Capacity, long p_TargetCapacity)
        {
            List<MemoryBlock> l_Blocks = new List<MemoryBlock>();

            long l_NeededBytes = p_TargetCapacity - p_Capacity;

            while (l_NeededBytes > 0)
            {

                // Find Best Pool
                StreamManagerPool l_Pool = m_ChooserPolicy.FindBestPool(p_Capacity, p_TargetCapacity);

                if (l_Pool == null)
                    throw new Exception($"No Pool found. Capacity {p_Capacity}, TargetCapacity {p_TargetCapacity}, Remaining Bytes {l_NeededBytes}");

                // Allocated all necessary blocks
                while (l_NeededBytes > 0)
                {

                    // If if max usage reached restart and search for a different pool
                    if (!m_ChooserPolicy.PoolHasFreeBlocks(l_Pool))
                        break;

                    MemoryBlock l_MemoryBlock = l_Pool.GetBlock();
                    l_NeededBytes = l_NeededBytes - l_MemoryBlock.GetLength();
                    l_Blocks.Add(l_MemoryBlock);
                }

            }

            return l_Blocks;

        }
    }
}
