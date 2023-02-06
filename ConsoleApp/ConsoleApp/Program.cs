using System;
using ConsoleApp.MappingTests;
using Npgsql;

// Mappers benchmarking below
/*
var benchmark = new MapperBenchmark();
benchmark.BenchmarkMappers();
benchmark.BenchmarkMappersOnlyMapping();
benchmark.BenchmarkMappersOnlyMappingEasyMapping();
*/


// DB code below
Console.WriteLine("Initiating db conn");
var cs = "Host=db;Username=postgres;Password=pw;Database=testDb";
using var con = new NpgsqlConnection(cs);
con.Open();
Console.WriteLine("Db initialized");
var sql = "SELECT version()";
using var cmd = new NpgsqlCommand(sql, con);
var version = cmd.ExecuteScalar().ToString();
Console.WriteLine($"PostgreSQL version: {version}");