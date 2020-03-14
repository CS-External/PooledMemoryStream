using System;

namespace MicroLikeAppFramework.PooledMemoryStreams.Pools
{
    public abstract class MemoryBlock
    {
        private StreamManagerPool m_Pool;

        protected MemoryBlock(StreamManagerPool p_Pool)
        {
            m_Pool = p_Pool;
        }

        public abstract int GetLength();
        public abstract Byte ReadByte(int p_Pos);
        public abstract void WriteByte(int p_Pos, Byte p_Value);
        public abstract void Read(int p_Pos, Byte[] p_Buffer, int p_Offset, int p_Count);
        public abstract void Write(int p_Pos, Byte[] p_Buffer, int p_Offset, int p_Count);

        public void ReturnBlock()
        {
            m_Pool.ReturnBlock(this);
        }
    }
}
