using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.PoolPolicies.Common;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;
using Xunit;

namespace PooledMemoryStream.Test.PoolPolicies.Common
{
    public class StreamManagerPoolIntervalCalculatorTest
    {

        [Fact]
        public void CalcIntervalTest()
        {

            List<StreamManagerPool> l_Pools = new List<StreamManagerPool>();
            l_Pools.Add(new StreamManagerArrayPool("1", 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("2", 10 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("3", 30 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("4", 100 * 1024, 1));

            StreamManagerPoolIntervalCalculator l_Calculator = new StreamManagerPoolIntervalCalculator();
            List<PoolChooserPolicyPoolItem> l_Intervals = l_Calculator.CalcIntervals(l_Pools);

            Assert.Equal(4, l_Intervals.Count);

            Assert.Equal(0, l_Intervals[0].Start);
            Assert.Equal(8192, l_Intervals[0].End);
            Assert.Equal(l_Pools[0], l_Intervals[0].Pool);

            Assert.Equal(8193, l_Intervals[1].Start);
            Assert.Equal(30720, l_Intervals[1].End);
            Assert.Equal(l_Pools[1], l_Intervals[1].Pool);

            Assert.Equal(30721, l_Intervals[2].Start);
            Assert.Equal(92160, l_Intervals[2].End);
            Assert.Equal(l_Pools[2], l_Intervals[2].Pool);

            Assert.Equal(92161, l_Intervals[3].Start);
            Assert.Equal(Int64.MaxValue, l_Intervals[3].End);
            Assert.Equal(l_Pools[3], l_Intervals[3].Pool);
        }

        [Fact]
        public void CalcIntervalMaxValueTest()
        {

            List<StreamManagerPool> l_Pools = new List<StreamManagerPool>();
            l_Pools.Add(new StreamManagerArrayPool("1", 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("2", 10 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("3", 30 * 1024, 1));
            l_Pools.Add(new StreamManagerArrayPool("4", Int32.MaxValue, 1));

            StreamManagerPoolIntervalCalculator l_Calculator = new StreamManagerPoolIntervalCalculator();
            List<PoolChooserPolicyPoolItem> l_Intervals = l_Calculator.CalcIntervals(l_Pools);

            Assert.Equal(4, l_Intervals.Count);

            Assert.Equal(0, l_Intervals[0].Start);
            Assert.Equal(8192, l_Intervals[0].End);
            Assert.Equal(l_Pools[0], l_Intervals[0].Pool);

            Assert.Equal(8193, l_Intervals[1].Start);
            Assert.Equal(30720, l_Intervals[1].End);
            Assert.Equal(l_Pools[1], l_Intervals[1].Pool);

            Assert.Equal(30721, l_Intervals[2].Start);
            Assert.Equal(645120, l_Intervals[2].End);
            Assert.Equal(l_Pools[2], l_Intervals[2].Pool);

            Assert.Equal(645121, l_Intervals[3].Start);
            Assert.Equal(Int64.MaxValue, l_Intervals[3].End);
            Assert.Equal(l_Pools[3], l_Intervals[3].Pool);
        }

    }
}
