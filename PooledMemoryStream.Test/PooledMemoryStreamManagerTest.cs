﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PooledMemoryStreams;
using PooledMemoryStreams.PoolPolicies;
using PooledMemoryStreams.Pools;
using PooledMemoryStreams.Pools.Array;
using Xunit;

namespace PooledMemoryStream.Test
{
    public class PooledMemoryStreamManagerTest
    {

        [Fact]
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
                Assert.Equal(l_ByteCount * 100000, (int)l_Stream.Length);
                Assert.Equal(l_ByteCount * 100000, (int)l_Stream.Position);

                l_Stream.Position = 0;
                

                using (StreamReader l_StreamReader = new StreamReader(l_Stream, l_Encoding, false, 1024, true))
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        string l_ReadLine = l_StreamReader.ReadLine();
                        Assert.Equal("Hallo", l_ReadLine);
                    }
                }
            }

        }

        private PooledMemoryStreamManager CreatePool()
        {
            List<StreamManagerPool> l_List = new List<StreamManagerPool>();
            l_List.Add(new StreamManagerArrayPool(1024, 1000));
            l_List.Add(new StreamManagerArrayPool(10 * 1024, 1000));
            l_List.Add(new StreamManagerArrayPool(30 * 1024, 1000));
            l_List.Add(new StreamManagerArrayPool(100 * 1024, 100));

            return PooledMemoryStreamManagerBuilder.CreatePool(l_List);
        }
    }
}