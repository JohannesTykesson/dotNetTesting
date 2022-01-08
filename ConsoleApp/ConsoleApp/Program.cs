using System;
using ConsoleApp.MappingTests;
using Npgsql;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmark = new MapperBenchmark();
            benchmark.BenchmarkMappers();
            benchmark.BenchmarkMappersOnlyMapping();
            benchmark.BenchmarkMappersOnlyMappingEasyMapping();
        }
    }
}
