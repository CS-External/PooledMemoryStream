using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams
{
    public class PooledMemoryStreamManager
    {
        private IPoolChooserPolicy m_ChooserPolicy;

        public PooledMemoryStreamManager(IPoolChooserPolicy p_ChooserPolicy)
        {
            m_ChooserPolicy = p_ChooserPolicy;
        }

        public StreamManagerPool GetPoolByName(String p_Name)
        {
            return m_ChooserPolicy.GetAllPools().FirstOrDefault(x => x.Name == p_Name);
        }

        public List<StreamManagerPool> GetAllPools()
        {
            return m_ChooserPolicy.GetAllPools();
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

        public Stream GetStream(long p_Capacity)
        {
            return new PooledMemoryStream(p_Capacity, this);
        }

        public Stream GetStream()
        {
            return new PooledMemoryStream(0, this);
        }
    }
}
