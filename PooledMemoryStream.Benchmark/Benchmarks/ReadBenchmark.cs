using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using PooledMemoryStreams;

namespace PooledMemoryStream.Benchmark.Benchmarks
{
    [MemoryDiagnoser]
    public class ReadBenchmark
    {
        private MemoryStream m_MemoryStream;
        private Stream m_PooledStream;
        private PooledMemoryStreamManager m_MemoryStreamManager;
        private Byte[] m_ReadBuffer;


        [GlobalSetup]
        public void Setup()
        {
            m_MemoryStreamManager = PooledMemoryStreamManagerBuilder.CreateMediumPool();
            m_PooledStream = m_MemoryStreamManager.GetStream();
            m_MemoryStream = new MemoryStream();
            m_ReadBuffer = new byte[256];

            FillStream(m_PooledStream);
            FillStream(m_MemoryStream);
        }

        private void FillStream(Stream p_Stream)
        {
            using (StreamWriter l_Writer = new StreamWriter(p_Stream, Encoding.UTF8, 1024, true))
            {
                for (int i = 0; i < 5000; i++)
                {
                    l_Writer.Write("Hello World");
                }

                l_Writer.Flush();
            }
        }

        [Benchmark(Baseline = true)]
        public void ReadMemoryStream()
        {
            m_MemoryStream.Position = 1000;
            m_MemoryStream.Read(m_ReadBuffer, 0, m_ReadBuffer.Length);
        }

        [Benchmark]
        public void ReadPooledStream()
        {
            m_PooledStream.Position = 1000;
            m_PooledStream.Read(m_ReadBuffer, 0, m_ReadBuffer.Length);
        }

    }
}
