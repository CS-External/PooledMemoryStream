using System;
using System.Collections.Generic;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams.PoolPolicies.Common
{
    public class StreamManagerPoolIntervalCalculator
    {
        private const int CONST_DEFAULT_MAX_BLOCKS_BEWTEEN = 20;

        private int m_MaxBlocksBetween;

        public StreamManagerPoolIntervalCalculator()
        {
            m_MaxBlocksBetween = CONST_DEFAULT_MAX_BLOCKS_BEWTEEN;
        }

        public StreamManagerPoolIntervalCalculator(int p_MaxBlocksBetween)
        {
            m_MaxBlocksBetween = p_MaxBlocksBetween;
        }

        public List<PoolChooserPolicyPoolItem> CalcIntervals(List<StreamManagerPool> p_Pools)
        {
            // Sort by BlockSize
            p_Pools.Sort(SortByBlockSize);
            List<PoolChooserPolicyPoolItem> l_Result = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Last = null;

            foreach (StreamManagerPool l_Pool in p_Pools)
            {
                PoolChooserPolicyPoolItem l_CurrentItem = new PoolChooserPolicyPoolItem();
                l_CurrentItem.Pool = l_Pool;
                l_Result.Add(l_CurrentItem);

                if (l_Last == null)
                {
                    // First Item
                    l_CurrentItem.Start = 0;
                    l_Last = l_CurrentItem;
                    continue;
                }

                int l_SpaceBetween = l_CurrentItem.Pool.GetBlockSize() - l_Last.Pool.GetBlockSize();

                if (l_SpaceBetween == 0)
                    throw new Exception($"Two Pool with Same BlockSize {l_CurrentItem.Pool.GetBlockSize()} found . Auto Interval Config not possible");

                int l_NewStartNear = (int)(l_SpaceBetween * 0.75);

                int l_NewStart = 0;

                for (int i = 0; i < m_MaxBlocksBetween; i++)
                {

                    if (l_NewStart < l_NewStartNear)
                    {
                        l_NewStart = l_NewStart + l_Last.Pool.GetBlockSize();
                    }
                    else
                    {
                        break;
                    }

                }
                
                l_Last.End = l_NewStart + l_Last.Pool.GetBlockSize();
                l_CurrentItem.Start = l_Last.End + 1;

                l_Last = l_CurrentItem;
            }

            l_Last.End = Int64.MaxValue;

            return l_Result;
        }


        private int SortByBlockSize(StreamManagerPool p_X, StreamManagerPool p_Y)
        {
            return p_X.GetBlockSize().CompareTo(p_X.GetBlockSize());
        }
    }
}
