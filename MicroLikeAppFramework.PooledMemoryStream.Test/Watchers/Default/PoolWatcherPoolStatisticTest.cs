using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default;
using Xunit;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Watchers.Default
{
    public class PoolWatcherPoolStatisticTest
    {
        [Fact]
        public void UpdateStateTest()
        {
            TestPoolStateProvider l_Provider = new TestPoolStateProvider(50);

            PoolWatcherPoolStatistic l_PoolStatistic = new PoolWatcherPoolStatistic(l_Provider);
            Assert.Equal(PoolWatcherPoolTrend.NothingChanged, l_PoolStatistic.State.Trend);
            Assert.False(l_PoolStatistic.State.LimitReached);
            Assert.Equal(0, l_PoolStatistic.State.BlocksInUse);

            for (int i = 0; i < PoolWatcherPoolStatistic.CONST_MAX_QUEUE_SIZE + 5; i++)
            {
                l_Provider.BlocksInUse = i / 2;
                l_PoolStatistic.UpdateState();
            }

            Assert.Equal(PoolWatcherPoolTrend.Growing, l_PoolStatistic.State.Trend);
            Assert.False(l_PoolStatistic.State.LimitReached);
            Assert.Equal(24, l_PoolStatistic.State.BlocksInUse);

            l_Provider.BlocksInUse = 55;
            l_PoolStatistic.UpdateState();
            Assert.Equal(PoolWatcherPoolTrend.Growing, l_PoolStatistic.State.Trend);
            Assert.True(l_PoolStatistic.State.LimitReached);
            Assert.Equal(55, l_PoolStatistic.State.BlocksInUse);


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

            public int MaxBlockCount
            {
                get
                {
                    return m_Max;
                }
            }
        }
    }
}
