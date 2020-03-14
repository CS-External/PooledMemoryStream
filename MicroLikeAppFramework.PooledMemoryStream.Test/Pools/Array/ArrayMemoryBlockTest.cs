using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools.Array
{
    public class ArrayMemoryBlockTest: MemoryBlockTestBase
    {
        protected override MemoryBlock DoCreateBlock(byte[] p_Content)
        {
            return new ArrayMemoryBlock(new StreamManagerArrayPool("test", 0, 0), p_Content, p_Content.Length);
        }
    }
}
