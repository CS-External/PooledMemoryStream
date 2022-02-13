using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Watchers.Default
{
    [TestClass]
    public class PoolWatcherPoolStatisticTest
    {
        [TestMethod]
        public void UpdateStateTest()
        {
            TestPoolStateProvider l_Provider = new TestPoolStateProvider(50);

            PoolWatcherPoolStatistic l_PoolStatistic = new PoolWatcherPoolStatistic(l_Provider);
            Assert.AreEqual(PoolWatcherPoolTrend.NothingChanged, l_PoolStatistic.State.Trend);
            Assert.IsFalse(l_PoolStatistic.State.LimitReached);
            Assert.AreEqual(0, l_PoolStatistic.State.BlocksInUse);

            for (int i = 0; i < PoolWatcherPoolStatistic.CONST_MAX_QUEUE_SIZE + 5; i++)
            {
                l_Provider.BlocksInUse = i / 2;
                l_PoolStatistic.UpdateState();
            }

            Assert.AreEqual(PoolWatcherPoolTrend.Growing, l_PoolStatistic.State.Trend);
            Assert.IsFalse(l_PoolStatistic.State.LimitReached);
            Assert.AreEqual(24, l_PoolStatistic.State.BlocksInUse);

            l_Provider.BlocksInUse = 55;
            l_PoolStatistic.UpdateState();
            Assert.AreEqual(PoolWatcherPoolTrend.Growing, l_PoolStatistic.State.Trend);
            Assert.IsTrue(l_PoolStatistic.State.LimitReached);
            Assert.AreEqual(55, l_PoolStatistic.State.BlocksInUse);


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
