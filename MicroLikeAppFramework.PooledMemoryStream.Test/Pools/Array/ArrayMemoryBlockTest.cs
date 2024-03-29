﻿using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.Array
{
    [TestClass]
    public class ArrayMemoryBlockTest: MemoryBlockTestBase
    {
        protected override MemoryBlock DoCreateBlock(byte[] p_Content)
        {
            return new ArrayMemoryBlock(new StreamManagerArrayPool("test", 0, 0), p_Content, p_Content.Length);
        }
    }
}
