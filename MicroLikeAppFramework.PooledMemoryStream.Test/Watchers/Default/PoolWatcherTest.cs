using System;
using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Watchers.Default
{
    [TestClass]
    public class PoolWatcherTest
    {
        [TestMethod]
        public void PoolStateChangedTest()
        {
            Boolean l_Executed = false;

            PoolWatcher l_Pool = new PoolWatcher();
            l_Pool.OnPoolStateChanged += (p_Pool, p_Args) =>
            {
                Assert.AreEqual(PoolWatcherPoolTrend.NothingChanged, p_Args.OldState.Trend);
                Assert.IsFalse(p_Args.OldState.LimitReached);
                Assert.AreEqual(PoolWatcherPoolTrend.NothingChanged, p_Args.NewState.Trend);
                Assert.IsTrue(p_Args.NewState.LimitReached);
                l_Executed = true;
            };

            StreamManagerPool l_Manager = new StreamManagerArrayPool("1", 1, 2);
            List<StreamManagerPool> l_List = new List<StreamManagerPool>();
            l_List.Add(l_Manager);

            Assert.IsFalse(l_Executed);
            l_Pool.Watch(l_List);
            Assert.IsFalse(l_Executed);

            MemoryBlock l_MemoryBlock = l_Manager.GetBlock();
            l_Pool.Watch(l_List);
            Assert.IsNotNull(l_MemoryBlock);

            MemoryBlock l_MemoryBlock2 = l_Manager.GetBlock();
            l_Pool.Watch(l_List);
            Assert.IsNotNull(l_MemoryBlock2);

            MemoryBlock l_MemoryBlock3 = l_Manager.GetBlock();
            l_Pool.Watch(l_List);
            Assert.IsNotNull(l_MemoryBlock3);

            Assert.IsTrue(l_Executed);

        }

    }
}
