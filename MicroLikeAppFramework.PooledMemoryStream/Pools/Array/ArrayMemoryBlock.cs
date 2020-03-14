using System;

namespace MicroLikeAppFramework.PooledMemoryStreams.Pools.Array
{
    public class ArrayMemoryBlock: MemoryBlock
    {
        private Byte[] m_Data;
        private int m_Length;

        public ArrayMemoryBlock(StreamManagerPool p_Pool, byte[] p_Data, int p_Length) : base(p_Pool)
        {
            m_Data = p_Data;
            m_Length = p_Length;
        }

        public override int GetLength()
        {
            return m_Length;
        }

        public override byte ReadByte(int p_Pos)
        {
            return m_Data[p_Pos];
        }

        public override void WriteByte(int p_Pos, Byte p_Value)
        {
            m_Data[p_Pos] = p_Value;
        }

        public override void Read(int p_Pos, byte[] p_Buffer, int p_Offset, int p_Count)
        {
            System.Array.Copy(m_Data, p_Pos, p_Buffer, p_Offset, p_Count);
        }

        public override void Write(int p_Pos, byte[] p_Buffer, int p_Offset, int p_Count)
        {
            System.Array.Copy(p_Buffer, p_Offset, m_Data, p_Pos, p_Count);
        }
    }
}
