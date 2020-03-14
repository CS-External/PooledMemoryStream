using System;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using MicroLikeAppFramework.PooledMemoryStreams;
using MicroLikeAppFramework.PooledMemoryStreams.Builders;

namespace MicroLikeAppFramework.PooledMemoryStream.Benchmark.Benchmarks
{
    [MemoryDiagnoser]
    public class CreateWriteDisposeBenchmark
    {
        private PooledMemoryStreamManager m_MemoryStreamManager;
        private Byte[] m_WriteBuffer;

        [GlobalSetup]
        public void Setup()
        {
            m_MemoryStreamManager = PooledMemoryStreamManagerBuilder.CreateLargePool();
            m_WriteBuffer = Encoding.UTF8.GetBytes("Hallo Welt");
        }

        [Benchmark(Baseline = true)]
        public void CreateWriteDisposeMemoryStream()
        {
            using (Stream l_Stream = new MemoryStream())
            {
                for (int i = 0; i < 100000; i++)
                {
                    l_Stream.Write(m_WriteBuffer, 0, m_WriteBuffer.Length);
                }
            }

        }

        [Benchmark]
        public void CreateWriteDisposePooledStream()
        {
            using (Stream l_Stream = m_MemoryStreamManager.GetStream())
            {
                for (int i = 0; i < 100000; i++)
                {
                    l_Stream.Write(m_WriteBuffer, 0, m_WriteBuffer.Length);
                }
            }
        }

    }
}
