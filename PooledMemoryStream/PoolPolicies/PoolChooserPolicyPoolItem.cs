using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies
{
    public class PoolChooserPolicyPoolItem
    {
        private long m_Start;
        private long m_End;

        public bool IsInRange(long p_Capacity)
        {
            return m_Start <= p_Capacity && m_End >= p_Capacity;
        }

        public long Start
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        public long End
        {
            get { return m_End; }
            set
            {
                m_End = value;
            }
            
        }

        public StreamManagerPool Pool { get; set; }
    }
}
