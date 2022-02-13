using System.Collections.Generic;
using MicroLikeAppFramework.PooledMemoryStreams.PoolPolicies;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.PoolPolicies
{
    [TestClass]
    public class FreeSpaceAwarePoolChooserPolicyTest
    {

        [TestMethod]
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

            FreeSpaceAwarePoolChooserPolicy l_Chooser = new FreeSpaceAwarePoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.AreEqual(l_Items[1].Pool, l_Pool);
            Assert.IsTrue(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }

        [TestMethod]
        public void FindBestFallBackToLastTest()
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
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 21;
            l_Item.End = 30;
            l_Item.Pool = new StreamManagerArrayPool("3", 1, 1);
            l_Items.Add(l_Item);

            FreeSpaceAwarePoolChooserPolicy l_Chooser = new FreeSpaceAwarePoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.AreEqual(l_Items[2].Pool, l_Pool);
            Assert.IsTrue(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }

        [TestMethod]
        public void FindBestFallBackToFristTest()
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
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 21;
            l_Item.End = 30;
            l_Item.Pool = new StreamManagerArrayPool("3", 1, 0);
            l_Items.Add(l_Item);

            FreeSpaceAwarePoolChooserPolicy l_Chooser = new FreeSpaceAwarePoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.AreEqual(l_Items[0].Pool, l_Pool);
            Assert.IsTrue(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }


        [TestMethod]
        public void FindBestNoFreeSpaceTest()
        {

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool("1", 1, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool("2", 1, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 21;
            l_Item.End = 30;
            l_Item.Pool = new StreamManagerArrayPool("3", 1, 0);
            l_Items.Add(l_Item);

            FreeSpaceAwarePoolChooserPolicy l_Chooser = new FreeSpaceAwarePoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(15, 15);

            Assert.IsNull(l_Pool);
        }


        [TestMethod]
        public void FindBestNothingMatchingTest()
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

            FreeSpaceAwarePoolChooserPolicy l_Chooser = new FreeSpaceAwarePoolChooserPolicy(l_Items);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(80, 80);

            Assert.AreEqual(l_Items[0].Pool, l_Pool);
            Assert.IsTrue(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }

        [TestMethod]
        public void FallBackTest()
        {
            StreamManagerArrayPool l_FallBack = new StreamManagerArrayPool("Fallback", 1, 1);

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool("1", 1, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool("2", 1, 0);
            l_Items.Add(l_Item);

            FreeSpaceAwarePoolChooserPolicy l_Chooser = new FreeSpaceAwarePoolChooserPolicy(l_Items, l_FallBack);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(25, 25);

            Assert.AreEqual(l_FallBack, l_Pool);
            Assert.IsTrue(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }

        [TestMethod]
        public void FallBackIgnoreFreeSpaceTest()
        {
            StreamManagerArrayPool l_FallBack = new StreamManagerArrayPool("Fallback", 1, 0);

            List<PoolChooserPolicyPoolItem> l_Items = new List<PoolChooserPolicyPoolItem>();
            PoolChooserPolicyPoolItem l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 0;
            l_Item.End = 10;
            l_Item.Pool = new StreamManagerArrayPool("1", 1, 0);
            l_Items.Add(l_Item);
            l_Item = new PoolChooserPolicyPoolItem();
            l_Item.Start = 11;
            l_Item.End = 20;
            l_Item.Pool = new StreamManagerArrayPool("2", 1, 0);
            l_Items.Add(l_Item);

            FreeSpaceAwarePoolChooserPolicy l_Chooser = new FreeSpaceAwarePoolChooserPolicy(l_Items, l_FallBack);
            StreamManagerPool l_Pool = l_Chooser.FindBestPool(25, 25);

            Assert.AreEqual(l_FallBack, l_Pool);
            Assert.IsTrue(l_Chooser.PoolHasFreeBlocks(l_Pool));
        }

    }
}
