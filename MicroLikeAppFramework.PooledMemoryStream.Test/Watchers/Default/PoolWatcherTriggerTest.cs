using System;
using System.Threading.Tasks;
using MicroLikeAppFramework.PooledMemoryStreams.Watchers.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Watchers.Default
{
    [TestClass]
    public class PoolWatcherTriggerTest
    {
        [TestMethod]
        public void TriggerTest()
        {
            Boolean l_Executed = false;

            PoolWatcherTrigger l_Watcher = new PoolWatcherTrigger(100);
            l_Watcher.Start(() => { l_Executed = true; });

            Task.Delay(150).Wait();
            Assert.IsTrue(l_Executed);

            l_Watcher.Stop();
            l_Executed = false;
            Task.Delay(150).Wait();

            Assert.IsFalse(l_Executed);
        }

    }
}
