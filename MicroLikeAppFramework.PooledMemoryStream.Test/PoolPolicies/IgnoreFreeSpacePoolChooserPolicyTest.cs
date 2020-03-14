using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using Xunit;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.PoolPolicies
{
    public class IgnoreFreeSpacePoolChooserPolicyTest
    {
        [Fact]
        public void FindBestTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool("1", 1, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool("2", 1, 1);
            l_Items.Add(l_Item);

            IgnoreFreeSpacePoolChooserPolicy l_Chooser = new IgnoreFreeSpacePoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.Equal(l_Items[1].Pool, l_Pool);
            Assert.True(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }


        [Fact]
        public void IgnoreFreeSpaceTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool("1", 1, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool("2", 1, 0);
            l_Items.Add(l_Item);

            IgnoreFreeSpacePoolChooserPolicy l_Chooser = new IgnoreFreeSpacePoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.Equal(l_Items[1].Pool, l_Pool);
            Assert.True(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }

        [Fact]
        public void FallBackTest()
        {
            StreamManagerArrayPool l_FallBack = new StreamManagerArrayPool("Fallback", 1, 1);

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool("1", 1, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool("2", 1, 0);
            l_Items.Add(l_Item);

            IgnoreFreeSpacePoolChooserPolicy l_Chooser = new IgnoreFreeSpacePoolChooserPolicy(l_Items, l_FallBack);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(25, 25);

            Assert.Equal(l_FallBack, l_Pool);
            Assert.True(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }

        [Fact]
        public void FallBackIgnoreFreeSpaceTest()
        {
            StreamManagerArrayPool l_FallBack = new StreamManagerArrayPool("Fallback", 1, 0);

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool("1", 1, 1);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool("2", 1, 0);
            l_Items.Add(l_Item);

            IgnoreFreeSpacePoolChooserPolicy l_Chooser = new IgnoreFreeSpacePoolChooserPolicy(l_Items, l_FallBack);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(25, 25);

            Assert.Equal(l_FallBack, l_Pool);
            Assert.True(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }
    }
}
