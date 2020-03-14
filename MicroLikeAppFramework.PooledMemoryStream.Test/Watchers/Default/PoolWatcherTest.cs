using System;
using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default;
using Xunit;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Watchers.Default
{
    public class PoolWatcherTest
    {
        [Fact]
        public void PoolStateChangedTest()
        {
            Boolean l_Executed = false;

            PoolWatcher l_Pool = new PoolWatcher();
            l_Pool.OnPoolStateChanged += (p_Pool, p_Args) =>
            {
                Assert.Equal(PoolWatcherPoolTrend.NothingChanged, p_Args.OldState.Trend);
                Assert.False(p_Args.OldState.LimitReached);
                Assert.Equal(PoolWatcherPoolTrend.NothingChanged, p_Args.NewState.Trend);
                Assert.True(p_Args.NewState.LimitReached);
                l_Executed = true;
            };

            StreamManagerPool l_Manager = new StreamManagerArrayPool("1", 1, 2);
            List<StreamManagerPool> l_List = new List<StreamManagerPool>();
            l_List.Add(l_Manager);

            Assert.False(l_Executed);
            l_Pool.Watch(l_List);
            Assert.False(l_Executed);

            MemoryBlock l_MemoryBlock = l_Manager.GetBlock();
            l_Pool.Watch(l_List);
            Assert.NotNull(l_MemoryBlock);

            MemoryBlock l_MemoryBlock2 = l_Manager.GetBlock();
            l_Pool.Watch(l_List);
            Assert.NotNull(l_MemoryBlock2);

            MemoryBlock l_MemoryBlock3 = l_Manager.GetBlock();
            l_Pool.Watch(l_List);
            Assert.NotNull(l_MemoryBlock3);

            Assert.True(l_Executed);

        }

    }
}
