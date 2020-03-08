using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public class PoolChooserPolicyPoolItem
    {
        public bool IsInRange(long p_Capacity)
        {
            return Start <= p_Capacity && End >= p_Capacity;
        }

        public long Start { get; set; }
        public long End { get; set; }
        public StreamManagerPool Pool { get; set; }
    }
}
