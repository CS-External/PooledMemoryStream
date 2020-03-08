using System;
using System.Collections.Generic;
using System.Text;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;
using Xunit;

namespace PooledMemoryStream.Test.PoolPolicies
{
    public class DefaultPoolChooserPolicyTest
    {

        [Fact]
        public void FindBestTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool(1, 1, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool(1, 1, 1);
            l_Items.Add(l_Item);

            DefaultPoolChooserPolicy l_Chooser = new DefaultPoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.Equal(l_Items[1].Pool, l_Pool);
        }

        [Fact]
        public void FindBestFallBackToLastTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 21;
            l_Item.End = 30;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 1);
            l_Items.Add(l_Item);

            DefaultPoolChooserPolicy l_Chooser = new DefaultPoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.Equal(l_Items[2].Pool, l_Pool);
        }

        [Fact]
        public void FindBestFallBackToFristTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 21;
            l_Item.End = 30;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 0);
            l_Items.Add(l_Item);

            DefaultPoolChooserPolicy l_Chooser = new DefaultPoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.Equal(l_Items[0].Pool, l_Pool);
        }


        [Fact]
        public void FindBestNoFreeSpaceTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 21;
            l_Item.End = 30;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 0);
            l_Items.Add(l_Item);

            DefaultPoolChooserPolicy l_Chooser = new DefaultPoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.Null(l_Pool);
        }


        [Fact]
        public void FindBestNothingMatchingTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool(1, 0, 0);
            l_Items.Add(l_Item);

            DefaultPoolChooserPolicy l_Chooser = new DefaultPoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(80, 80);

            Assert.Equal(l_Items[0].Pool, l_Pool);
        }

    }
}
