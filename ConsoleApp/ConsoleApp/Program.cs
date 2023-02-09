using System;
using ConsoleApp.MappingTests;
using ConsoleApp.PostgreSQL;
using Npgsql;

Console.WriteLine("Starting Program");

// Mappers benchmarking below
/*
var benchmark = new MapperBenchmark();
benchmark.BenchmarkMappers();
benchmark.BenchmarkMappersOnlyMapping();
benchmark.BenchmarkMappersOnlyMappingEasyMapping();
*/


// manual DB code below
/*
Console.WriteLine("Initiating db conn");
var cs = "Host=db;Username=postgres;Password=pw;Database=testDb";
using var con = new NpgsqlConnection(cs);
con.Open();
Console.WriteLine("Db initialized");
var sql = "SELECT version()";
using var cmd = new NpgsqlCommand(sql, con);
var version = cmd.ExecuteScalar().ToString();
Console.WriteLine($"PostgreSQL version: {version}");
*/

var dbRunner = new DatabaseRunner();
await dbRunner.CheckInfoInDb();
await dbRunner.SeedDatabase();
await dbRunner.Benchmark();

Console.WriteLine("Stopping program");