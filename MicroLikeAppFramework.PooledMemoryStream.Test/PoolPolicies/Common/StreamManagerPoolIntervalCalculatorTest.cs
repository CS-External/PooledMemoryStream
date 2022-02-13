using System;
using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies;
using MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies.Common;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.PoolPolicies.Common
{
    [TestClass]
    public class StreamManagerPoolIntervalCalculatorTest
    {

        [TestMethod]
        public void CalcIntervalTest()
        {

            List<StreamManagerPool> l_Pools = new List<StreamManagerPool>();
            l_Pools.Add(new StreamManagerArrayPool("1", 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("2", 10 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("3", 30 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("4", 100 * 1024, 1));

            StreamManagerPoolIntervalCalculator l_Calculator = new StreamManagerPoolIntervalCalculator();
            List<PoolChooserPolicyPoolItem> l_Intervals = l_Calculator.CalcIntervals(l_Pools);

            Assert.AreEqual(4, l_Intervals.Count);

            Assert.AreEqual(0, l_Intervals[0].Start);
            Assert.AreEqual(8192, l_Intervals[0].End);
            Assert.AreEqual(l_Pools[0], l_Intervals[0].Pool);

            Assert.AreEqual(8193, l_Intervals[1].Start);
            Assert.AreEqual(30720, l_Intervals[1].End);
            Assert.AreEqual(l_Pools[1], l_Intervals[1].Pool);

            Assert.AreEqual(30721, l_Intervals[2].Start);
            Assert.AreEqual(92160, l_Intervals[2].End);
            Assert.AreEqual(l_Pools[2], l_Intervals[2].Pool);

            Assert.AreEqual(92161, l_Intervals[3].Start);
            Assert.AreEqual(Int64.MaxValue, l_Intervals[3].End);
            Assert.AreEqual(l_Pools[3], l_Intervals[3].Pool);
        }

        [TestMethod]
        public void CalcIntervalMaxValueTest()
        {

            List<StreamManagerPool> l_Pools = new List<StreamManagerPool>();
            l_Pools.Add(new StreamManagerArrayPool("1", 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("2", 10 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("3", 30 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("4", Int32.MaxValue, 1));

            StreamManagerPoolIntervalCalculator l_Calculator = new StreamManagerPoolIntervalCalculator();
            List<PoolChooserPolicyPoolItem> l_Intervals = l_Calculator.CalcIntervals(l_Pools);

            Assert.AreEqual(4, l_Intervals.Count);

            Assert.AreEqual(0, l_Intervals[0].Start);
            Assert.AreEqual(8192, l_Intervals[0].End);
            Assert.AreEqual(l_Pools[0], l_Intervals[0].Pool);

            Assert.AreEqual(8193, l_Intervals[1].Start);
            Assert.AreEqual(30720, l_Intervals[1].End);
            Assert.AreEqual(l_Pools[1], l_Intervals[1].Pool);

            Assert.AreEqual(30721, l_Intervals[2].Start);
            Assert.AreEqual(645120, l_Intervals[2].End);
            Assert.AreEqual(l_Pools[2], l_Intervals[2].Pool);

            Assert.AreEqual(645121, l_Intervals[3].Start);
            Assert.AreEqual(Int64.MaxValue, l_Intervals[3].End);
            Assert.AreEqual(l_Pools[3], l_Intervals[3].Pool);
        }

    }
}
