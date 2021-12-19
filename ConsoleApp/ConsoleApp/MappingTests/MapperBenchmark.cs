using System.Collections.Generic;
using AutoMapper;
using Mapster;
using System.Diagnostics;
using System;

namespace ConsoleApp.MappingTests
{
    class MapperBenchmark
    {
        public static Person person;

        public MapperBenchmark()
        {
            person = new Person()
            {
                Age = 23,
                FirstName = "firstname",
                LastName = "lastname",
                IrrelevantInformation = new List<string>() {"one", "two", "three"},
                Relatives = new List<Person>() { new Person(), new Person(), new Person() },
                Spouse = new Person() {LastName = "spouseLastname"}
            };
        }

        public void BenchmarkMappers()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            TestMapster();
            sw.Stop();
            Console.WriteLine("Mapster Elapsed={0}", sw.Elapsed);

            sw.Reset();

            sw.Start();
            TestAutoMapper();
            sw.Stop();
            Console.WriteLine("Automapper Elapsed={0}", sw.Elapsed);
        }

        public void TestMapster()
        {
            var config = new TypeAdapterConfig();
            config.NewConfig<Person, PersonDto>()
                .Ignore(dest => dest.IrrelevantInformation)
                .Map(dest => dest.Name,
                    src => $"{src.FirstName} {src.LastName}");
            var mapper = new MapsterMapper.Mapper(config);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var personDto = mapper.Map<PersonDto>(person);
            Console.WriteLine(personDto.ToString());
            sw.Stop();
            Console.WriteLine("Mapster in method Elapsed={0}", sw.Elapsed);
        }

        public void TestAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDto>()
                    .ForMember(p => p.IrrelevantInformation, opt => opt.Ignore())
                    .ForMember(p => p.Name, opt => opt.MapFrom(s => string.Concat(s.FirstName, " ", s.LastName)))
                    .ForMember(p => p.SpouseAge, opt => opt.MapFrom(dto => dto.Spouse.Age))
                    .ForMember(p => p.SpouseName, opt => opt.MapFrom(dto => dto.Spouse.LastName));
            });
            IMapper mapper = new Mapper(config);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var personDto = mapper.Map<PersonDto>(person);
            Console.WriteLine(personDto.ToString());
            sw.Stop();
            Console.WriteLine("Automapper in method Elapsed={0}", sw.Elapsed);
        }
    }
}
