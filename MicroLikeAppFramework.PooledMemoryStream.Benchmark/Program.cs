using BenchmarkDotNet.Running;

namespace MicroLikeAppFramework.PooledMemoryStream.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
