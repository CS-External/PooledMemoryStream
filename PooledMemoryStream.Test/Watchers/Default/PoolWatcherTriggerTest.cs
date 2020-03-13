﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PooledMemoryStreams.Watchers.Default;
using Xunit;

namespace PooledMemoryStream.Test.Watchers.Default
{
    public class PoolWatcherTriggerTest
    {
        [Fact]
        public void TriggerTest()
        {
            Boolean l_Executed = false;

            PoolWatcherTrigger l_Watcher = new PoolWatcherTrigger(100);
            l_Watcher.Start(() => { l_Executed = true; });

            Task.Delay(150).Wait();
            Assert.True(l_Executed);

            l_Watcher.Stop();
            l_Executed = false;
            Task.Delay(150).Wait();

            Assert.False(l_Executed);
        }

    }
}
