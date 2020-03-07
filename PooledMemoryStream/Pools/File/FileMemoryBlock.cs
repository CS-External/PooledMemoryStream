using System;
using System.Collections.Generic;
using System.Text;

namespace PooledMemoryStreams.Pools.Stream
{
    public class FileMemoryBlock: MemoryBlock
    {
        private System.IO.Stream m_Stream;

        public FileMemoryBlock(StreamManagerPool p_Pool, System.IO.Stream p_Stream) : base(p_Pool)
        {
            m_Stream = p_Stream;
        }

        public override int GetLength()
        {
            return Int32.MaxValue;
        }

        public override byte ReadByte(int p_Pos)
        {
            m_Stream.Position = p_Pos;
            return (byte)m_Stream.ReadByte();
        }

        public override void WriteByte(int p_Pos, byte p_Value)
        {
            m_Stream.Position = p_Pos;
            m_Stream.WriteByte(p_Value);
        }

        public override void Read(int p_Pos, byte[] p_Buffer, int p_Offset, int p_Count)
        {
            m_Stream.Position = p_Pos;
            m_Stream.Read(p_Buffer, p_Offset, p_Count);
        }

        public override void Write(int p_Pos, byte[] p_Buffer, int p_Offset, int p_Count)
        {
            m_Stream.Position = p_Pos;
            m_Stream.Write(p_Buffer, p_Offset, p_Count);
        }
    }
}
