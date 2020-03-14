using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Watchers.Default;
using Xunit;

namespace PooledMemoryStream.Test.Watchers.Default
{
    public class PoolWatcherPoolStatisticTest
    {
        [Fact]
        public void UpdateStateTest()
        {
            TestPoolStateProvider l_Provider = new TestPoolStateProvider(2);

            PoolWatcherPoolStatistic l_PoolStatistic = new PoolWatcherPoolStatistic(l_Provider);
            l_PoolStatistic.UpdateState();
            Assert.Equal(l_PoolStatistic.State, PoolWatcherPoolState.NothingChanged);

            l_Provider.BlocksInUse = 1;
            l_PoolStatistic.UpdateState();
            Assert.Equal(l_PoolStatistic.State, PoolWatcherPoolState.Growing);

            l_Provider.BlocksInUse = 4;
            l_PoolStatistic.UpdateState();
            Assert.Equal(l_PoolStatistic.State, PoolWatcherPoolState.LimitExceededAndGrowing);

            l_Provider.BlocksInUse = 4;
            l_PoolStatistic.UpdateState();
            Assert.Equal(l_PoolStatistic.State, PoolWatcherPoolState.LimitExceededAndGrowing);

            l_Provider.BlocksInUse = 4;
            l_PoolStatistic.UpdateState();
            Assert.Equal(l_PoolStatistic.State, PoolWatcherPoolState.LimitExceededAndGrowing);

            l_Provider.BlocksInUse = 4;
            l_PoolStatistic.UpdateState();
            Assert.Equal(l_PoolStatistic.State, PoolWatcherPoolState.LimitExceeded);

        }

        private class TestPoolStateProvider: IPoolStateProvider
        {
            public int BlocksInUse = 0;
            private int m_Max;

            public TestPoolStateProvider(int p_Max)
            {
                m_Max = p_Max;
            }

            public int GetBlocksInUse()
            {
                return BlocksInUse;
            }

            public bool HasFreeBlocks()
            {
                return BlocksInUse < m_Max;
            }
        }
    }
}
