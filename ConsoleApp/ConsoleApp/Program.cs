using System;
using ConsoleApp.MappingTests;
using Npgsql;
using ConsoleApp.MappingTests;



namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmark = new MapperBenchmark();
            benchmark.BenchmarkMappers();
        }
    }
}
