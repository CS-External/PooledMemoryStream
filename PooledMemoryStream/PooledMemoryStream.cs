using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PooledMemoryStreams.Common;
using PooledMemoryStreams.Pools;

namespace PooledMemoryStreams
{
    public class PooledMemoryStream: Stream
    {
        private PooledMemoryStreamBlockAndOffset m_BlockAndOffset;
        private PooledMemoryStreamDataBlock m_AccessBlock;
        private List<PooledMemoryStreamDataBlock> m_DataBlocks;
        private PooledMemoryStreamManager m_StreamManager;

        private long m_Length;
        private long m_Capacity;
        private long m_Position;

        public PooledMemoryStream()
        {
        }

        public PooledMemoryStream(long p_Capacity, PooledMemoryStreamManager p_StreamManager)
        {
            m_StreamManager = p_StreamManager;
            m_DataBlocks = new List<PooledMemoryStreamDataBlock>();
            m_AccessBlock = new PooledMemoryStreamDataBlock();
            m_BlockAndOffset = new PooledMemoryStreamBlockAndOffset();
            EnsureCapacity(p_Capacity);
        }

        public override int ReadByte()
        {
            long l_BytesRemaining = m_Length - m_Position;

            if (l_BytesRemaining <= 0)
            {
                return -1;
            }

            FindCurrentBlockAndOffset();
            return (int)m_BlockAndOffset.Block.ReadByte(m_BlockAndOffset.Offset);
        }

        public override void WriteByte(byte p_Value)
        {
            long l_EndPosition = Position + 1;

            EnsureCapacity(l_EndPosition);

            FindCurrentBlockAndOffset();

            m_BlockAndOffset.Block.WriteByte(m_BlockAndOffset.Offset, p_Value);

            if (m_Length < m_Position)
            {
                m_Length = m_Position;
            }

            m_Position = l_EndPosition;
        }

        public override void Flush()
        {

        }

        public override long Seek(long p_Offset, SeekOrigin p_Origin)
        {
            long l_NewPosition;

            switch (p_Origin)
            {
                case SeekOrigin.Begin:
                    l_NewPosition = p_Offset;
                    break;
                case SeekOrigin.Current:
                    l_NewPosition = p_Offset + m_Position;
                    break;
                case SeekOrigin.End:
                    l_NewPosition = p_Offset + m_Length;
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin", nameof(p_Origin));
            }
            if (l_NewPosition < 0)
            {
                throw new IOException("Seek before beginning");
            }

            m_Position = l_NewPosition;
            return m_Position;
        }

        public override void SetLength(long p_Value)
        {
            if (p_Value < 0)
                throw new ArgumentException("Value must be positive", nameof(p_Value));

            if (p_Value == 0)
            {
                foreach (PooledMemoryStreamDataBlock l_DataBlock in m_DataBlocks)
                {
                    l_DataBlock.SourceBlock.ReturnBlock();
                }
                m_DataBlocks.Clear();
                m_Capacity = 0;
            }
            else
            {
                EnsureCapacity(p_Value);
            }


            m_Length = p_Value;

            if (m_Position > m_Length)
            {
                this.m_Position = m_Length;
            }
        }



        public override int Read(byte[] p_Buffer, int p_Offset, int p_Count)
        {
            int l_BytesRemaining = (int)(m_Length - m_Position);

            if (l_BytesRemaining <= 0)
            {
                return 0;
            }

            l_BytesRemaining = Math.Min(p_Count, l_BytesRemaining);

            int l_BytesReaded = 0;
            FindCurrentBlockAndOffset();

            while (true)
            {

                int l_AmountToCopy = Math.Min(m_BlockAndOffset.Block.Length, l_BytesRemaining);
                m_BlockAndOffset.Block.Read(m_BlockAndOffset.Offset, p_Buffer, p_Offset, l_AmountToCopy);
                l_BytesReaded = l_BytesReaded + l_AmountToCopy;
                l_BytesRemaining = l_BytesRemaining - l_AmountToCopy;
                p_Offset = p_Offset + l_AmountToCopy;

                if (l_BytesRemaining > 0)
                {
                    m_BlockAndOffset.BlockIndex++;

                    if (m_DataBlocks.Count <= m_BlockAndOffset.BlockIndex)
                        break;

                    m_BlockAndOffset.Block = m_DataBlocks[m_BlockAndOffset.BlockIndex];
                    m_BlockAndOffset.Offset = 0;

                }
                else
                {
                    break;
                }
            }

            m_Position = m_Position + l_BytesReaded;
            return l_BytesReaded;
        }

        private void FindCurrentBlockAndOffset()
        {
            m_AccessBlock.Start = m_Position;
            m_BlockAndOffset.BlockIndex = m_DataBlocks.BinarySearch(m_AccessBlock, PooledMemoryStreamDataBlockComparer.Instance);
            m_BlockAndOffset.Block = m_DataBlocks[m_BlockAndOffset.BlockIndex];
            m_BlockAndOffset.Offset = (int)(m_Position - m_BlockAndOffset.Block.Start);
        }

        public override void Write(byte[] p_Buffer, int p_Offset, int p_Count)
        {
            long l_EndPosition = m_Position + p_Count;

            EnsureCapacity(l_EndPosition);

            FindCurrentBlockAndOffset();

            if (p_Count == 1)
            {
                m_BlockAndOffset.Block.WriteByte(m_BlockAndOffset.Offset, p_Buffer[p_Offset]);
            }
            else
            {
                int l_BytesRemaining = p_Count;

                while (true)
                {
                    int l_AmountToCopy = Math.Min(m_BlockAndOffset.Block.Length, l_BytesRemaining);
                    m_BlockAndOffset.Block.Write(m_BlockAndOffset.Offset, p_Buffer, p_Offset, l_AmountToCopy);
                    l_BytesRemaining = l_BytesRemaining - l_AmountToCopy;
                    p_Offset = p_Offset + l_AmountToCopy;

                    if (l_BytesRemaining > 0)
                    {
                        m_BlockAndOffset.BlockIndex++;
                        m_BlockAndOffset.Block = m_DataBlocks[m_BlockAndOffset.BlockIndex];
                        m_BlockAndOffset.Offset = 0;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            if (m_Length < m_Position)
            {
                m_Length = m_Position;
            }

            m_Position = l_EndPosition;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get
            {
                return m_Length;
            }
        }

        public override long Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        protected override void Dispose(bool p_Disposing)
        {

            if (p_Disposing)
            {
                GC.SuppressFinalize(this);
            }
            else
            {
                // We're being finalized.
                if (AppDomain.CurrentDomain.IsFinalizingForUnload())
                {
                    // If we're being finalized because of a shutdown, don't go any further.
                    // We have no idea what's already been cleaned up.
                    base.Dispose(p_Disposing);
                    return;
                }
            }

            foreach (PooledMemoryStreamDataBlock l_DataBlock in m_DataBlocks)
            {
                l_DataBlock.SourceBlock.ReturnBlock();
            }

            m_DataBlocks.Clear();

            m_StreamManager = null;
            m_DataBlocks = null;

            base.Dispose(p_Disposing);
        }

        ~PooledMemoryStream()
        {
            this.Dispose(false);
        }

        public override void Close()
        {
            Dispose(true);
        }

        private void EnsureCapacity(long p_Value)
        {
            if (m_Capacity > p_Value)
                return;

            List<MemoryBlock> l_Blocks = m_StreamManager.GetBlock(m_Capacity, p_Value);

            PooledMemoryStreamDataBlock l_LastBock;

            if (m_DataBlocks.Count != 0)
            {
                l_LastBock = m_DataBlocks[m_DataBlocks.Count - 1];
            }
            else
            {
                l_LastBock = PooledMemoryStreamDataBlock.EMPTY;
            }

            foreach (MemoryBlock l_Block in l_Blocks)
            {
                PooledMemoryStreamDataBlock l_DataBlock = new PooledMemoryStreamDataBlock();
                l_DataBlock.SourceBlock = l_Block;
                l_DataBlock.Read = l_Block.Read;
                l_DataBlock.Write = l_Block.Write;
                l_DataBlock.ReadByte = l_Block.ReadByte;
                l_DataBlock.WriteByte = l_Block.WriteByte;
                l_DataBlock.Length = l_Block.GetLength();
                l_DataBlock.Start = l_LastBock.Start + l_LastBock.Length + 1;
                m_DataBlocks.Add(l_DataBlock);
                m_Capacity = m_Capacity + l_DataBlock.Length;

                l_LastBock = l_DataBlock;
            }

        }
    }
}
