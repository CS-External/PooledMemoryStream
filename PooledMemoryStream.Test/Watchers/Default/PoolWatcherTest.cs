using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;
using PooledMemoryStreams.Watchers.Default;
using Xunit;

namespace PooledMemoryStream.Test.Watchers.Default
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
                Assert.Equal(p_Args.OldState, PoolWatcherPoolState.NothingChanged);
                Assert.Equal(p_Args.NewState, PoolWatcherPoolState.Growing);
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


            Assert.True(l_Executed);

        }

    }
}
