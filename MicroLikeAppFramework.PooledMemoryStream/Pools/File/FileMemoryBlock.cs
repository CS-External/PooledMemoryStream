using System;
using System.IO;

namespace MicroLikeAppFramework.PooledMemoryStreams.Pools.File
{
    public class FileMemoryBlock: MemoryBlock
    {
        private System.IO.Stream m_Stream;
        private long m_Offset;
        private StreamManagerFilePoolDeleteHandler m_DeleteHandler;

        public StreamManagerFilePoolDeleteHandler DeleteHandler
        {
            get { return m_DeleteHandler; }
        }

        public FileMemoryBlock(StreamManagerPool p_Pool, Stream p_Stream, long p_Offset, StreamManagerFilePoolDeleteHandler p_DeleteHandler) : base(p_Pool)
        {
            m_Stream = p_Stream;
            m_Offset = p_Offset;
            m_DeleteHandler = p_DeleteHandler;
        }

        public override int GetLength()
        {
            return Int32.MaxValue;
        }

        public override byte ReadByte(int p_Pos)
        {
            long l_RealPosition = CalcRealPosition(p_Pos);

            lock (m_Stream)
            {
                if (m_Stream.Position != l_RealPosition)
                    m_Stream.Position = l_RealPosition;

                return (byte)m_Stream.ReadByte();
            }

        }

        private long CalcRealPosition(int p_Pos)
        {
            return m_Offset + p_Pos;
        }

        public override void WriteByte(int p_Pos, byte p_Value)
        {
            long l_RealPosition = CalcRealPosition(p_Pos);

            lock (m_Stream)
            {
                if (m_Stream.Position != l_RealPosition)
                    m_Stream.Position = l_RealPosition;

                m_Stream.WriteByte(p_Value);
            }

        }

        public override void Read(int p_Pos, byte[] p_Buffer, int p_Offset, int p_Count)
        {
            long l_RealPosition = CalcRealPosition(p_Pos);

            lock (m_Stream)
            {
                if (m_Stream.Position != l_RealPosition)
                    m_Stream.Position = l_RealPosition;

                m_Stream.Read(p_Buffer, p_Offset, p_Count);
            }

        }

        public override void Write(int p_Pos, byte[] p_Buffer, int p_Offset, int p_Count)
        {
            long l_RealPosition = CalcRealPosition(p_Pos);

            lock (m_Stream)
            {
                if (m_Stream.Position != l_RealPosition)
                    m_Stream.Position = l_RealPosition;

                m_Stream.Write(p_Buffer, p_Offset, p_Count);
            }

        }
    }
}
