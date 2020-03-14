using System;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;

namespace MicroLikeAppFramework.PooledMemoryStreams.Common
{
    public class PooledMemoryStreamDataBlock
    {
        public delegate Byte ReadByteDelegate(int p_Pos);
        public delegate void WriteByteDelegate(int p_Pos, Byte p_Value);
        public delegate void ReadDelegate(int p_Pos, Byte[] p_Buffer, int p_Offset, int p_Count);
        public delegate void WriteDelegate(int p_Pos, Byte[] p_Buffer, int p_Offset, int p_Count);

        internal static PooledMemoryStreamDataBlock EMPTY = new PooledMemoryStreamDataBlock();

        public ReadByteDelegate ReadByte;
        public WriteByteDelegate WriteByte;
        public ReadDelegate Read;
        public WriteDelegate Write;
        public long Start;
        public int Length;
        public MemoryBlock SourceBlock;
    }
}
