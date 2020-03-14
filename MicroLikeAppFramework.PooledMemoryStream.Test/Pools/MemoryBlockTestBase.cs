using System;
using MicroLikeAppFramework.PooledMemoryStreams.Pools;
using Xunit;

namespace MicroLikeAppFramework.PooledMemoryStream.Test.Pools
{
    public abstract class MemoryBlockTestBase
    {
        

        [Fact]
        public void ReadByteTest()
        {
            MemoryBlock l_MemoryBlock = CreateBlock();
            byte l_ReadByte = l_MemoryBlock.ReadByte(10);

            Assert.Equal(10, l_ReadByte);
        }

        [Fact]
        public void WriteByteTest()
        {
            MemoryBlock l_MemoryBlock = CreateBlock();
            l_MemoryBlock.WriteByte(10, 165);

            byte l_ReadByte = l_MemoryBlock.ReadByte(10);
            Assert.Equal(165, l_ReadByte);
        }

        [Fact]
        public void ReadTest()
        {
            MemoryBlock l_MemoryBlock = CreateBlock();

            Byte[] l_Buffer = new byte[10];
            l_MemoryBlock.Read(0, l_Buffer, 0, l_Buffer.Length);

            for (int i = 0; i < l_Buffer.Length; i++)
            {
                Assert.Equal(i, l_Buffer[i]);
            }
            
        }

        [Fact]
        public void WriteTest()
        {
            MemoryBlock l_MemoryBlock = CreateBlock();

            Byte[] l_WriteBuffer = new byte[10];
            for (int i = 0; i < l_WriteBuffer.Length; i++)
            {
                l_WriteBuffer[i] = 156;
            }

            l_MemoryBlock.Write(0, l_WriteBuffer, 0, l_WriteBuffer.Length);

            Byte[] l_Buffer = new byte[10];
            l_MemoryBlock.Read(0, l_Buffer, 0, l_Buffer.Length);

            for (int i = 0; i < l_Buffer.Length; i++)
            {
                Assert.Equal(156, l_Buffer[i]);
            }
        }

        public MemoryBlock CreateBlock()
        {
            Byte[] l_Bytes = new byte[265];

            for (int i = 0; i < l_Bytes.Length; i++)
            {
                l_Bytes[i] = (byte)i;
            }

            return DoCreateBlock(l_Bytes);
        }

        protected abstract MemoryBlock DoCreateBlock(Byte[] p_Content);
    }
}
