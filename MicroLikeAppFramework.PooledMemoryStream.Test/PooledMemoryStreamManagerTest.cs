using System;
using System.IO;
using System.Text;
using MicroLikeAppFramework.PooledMemoryStreams;
using MicroLikeAppFramework.PooledMemoryStreams.Builders;
using MicroLikeAppFramework.PooledMemoryStreams.Pools.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroLikeAppFramework.PooledMemoryStream.Test
{
    [TestClass]
    public class PooledMemoryStreamManagerTest
    {

        [TestMethod]
        public void WriteAndReadTest()
        {
            UTF8Encoding l_Encoding = new UTF8Encoding(false);

            PooledMemoryStreamManager l_Manager = CreatePool();

            using (Stream l_Stream = l_Manager.GetStream())
            {
                using (StreamWriter l_StreamWriter = new StreamWriter(l_Stream, l_Encoding, 1024, true))
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        l_StreamWriter.WriteLine("Hallo");
                    }

                    l_StreamWriter.Flush();
                }

                int l_ByteCount = l_Encoding.GetByteCount("Hallo" + Environment.NewLine);
                Assert.AreEqual(l_ByteCount * 100000, (int)l_Stream.Length);
                Assert.AreEqual(l_ByteCount * 100000, (int)l_Stream.Position);

                l_Stream.Position = 0;
                

                using (StreamReader l_StreamReader = new StreamReader(l_Stream, l_Encoding, false, 1024, true))
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        string l_ReadLine = l_StreamReader.ReadLine();
                        Assert.AreEqual("Hallo", l_ReadLine);
                    }
                }
            }

        }

        private PooledMemoryStreamManager CreatePool()
        {
            return PooledMemoryStreamManagerBuilder.Create()
                .AddPool(new StreamManagerArrayPool("1", 1024, 1000))
                .AddPool(new StreamManagerArrayPool("2", 10 * 1024, 1000))
                .AddPool(new StreamManagerArrayPool("3", 30 * 1024, 1000))
                .AddPool(new StreamManagerArrayPool("4", 100 * 1024, 100))
                .Build();
        }
    }
}
