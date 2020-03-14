using System.Collections.Generic;

namespace MicroLikeAppFramework.PooledMemoryStreams.Common
{
    public class PooledMemoryStreamDataBlockComparer : IComparer<PooledMemoryStreamDataBlock>
    {
        public static readonly PooledMemoryStreamDataBlockComparer Instance = new PooledMemoryStreamDataBlockComparer();

        public int Compare(PooledMemoryStreamDataBlock p_X, PooledMemoryStreamDataBlock p_Y)
        {
            return p_X.Start.CompareTo(p_Y.Start);
        }
    }
}
