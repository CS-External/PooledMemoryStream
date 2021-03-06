using System;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using MicroLikeAppFramework.PooledMemoryStreams;
using MicroLikeAppFramework.PooledMemoryStreams.Builders;

namespace MicroLikeAppFramework.PooledMemoryStream.Benchmark.Benchmarks
{
    [MemoryDiagnoser]
    public class WriteBenchmark
    {

        private MemoryStream m_MemoryStream;
        private Stream m_PooledStream;
        private PooledMemoryStreamManager m_MemoryStreamManager;
        private Byte[] m_WriteBuffer;

        [GlobalSetup]
        public void Setup()
        {
            m_MemoryStreamManager = PooledMemoryStreamManagerBuilder.CreateLargePool();
            m_PooledStream = m_MemoryStreamManager.GetStream();
            m_MemoryStream = new MemoryStream();
            m_WriteBuffer = Encoding.UTF8.GetBytes("Hallo Welt");
        }

        [Benchmark(Baseline = true)]
        public void WriteMemoryStream()
        {
            m_MemoryStream.SetLength(0);

            for (int i = 0; i < 100000 ; i++)
            {
                m_MemoryStream.Write(m_WriteBuffer, 0, m_WriteBuffer.Length);    
            }

        }

        [Benchmark]
        public void WritePooledStream()
        {
            m_PooledStream.SetLength(0);

            for (int i = 0; i < 100000; i++)
            {
                m_PooledStream.Write(m_WriteBuffer, 0, m_WriteBuffer.Length);
            }
        }



    }
}
