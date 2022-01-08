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
        public static EasyPerson easyPerson;

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
            easyPerson = new EasyPerson() {
                Age = 33,
                Name = "firstname",
                SpouseAge = 22,
                SpouseName = "spouseName"
            };
        }

        public void BenchmarkMappers()
        {
            Stopwatch sw = new Stopwatch();
            double time = 0;
            for (var i = 0; i< 10000; i++) {
                sw.Start();
                var config = new TypeAdapterConfig();
                config.NewConfig<Person, PersonDto>()
                    .Ignore(dest => dest.IrrelevantInformation)
                    .Map(dest => dest.Name,
                        src => $"{src.FirstName} {src.LastName}");
                var mapper = new MapsterMapper.Mapper(config);
                var personDto = mapper.Map<PersonDto>(person);
                sw.Stop();
                time += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
            }
            Console.WriteLine("Mapster Elapsed={0}", time);

            sw.Reset();

            time = 0;
            for (var i = 0; i< 10000; i++) {
                sw.Start();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Person, PersonDto>()
                        .ForMember(p => p.IrrelevantInformation, opt => opt.Ignore())
                        .ForMember(p => p.Name, opt => opt.MapFrom(s => string.Concat(s.FirstName, " ", s.LastName)))
                        .ForMember(p => p.SpouseAge, opt => opt.MapFrom(dto => dto.Spouse.Age))
                        .ForMember(p => p.SpouseName, opt => opt.MapFrom(dto => dto.Spouse.LastName));
                });
                IMapper mapper = new Mapper(config);
                var personDto = mapper.Map<PersonDto>(person);
                sw.Stop();
                time += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
            }
            Console.WriteLine("Automapper Elapsed={0}", time);
        }

        public void BenchmarkMappersOnlyMapping()
        {
            Stopwatch sw = new Stopwatch();
            double time = 0;
            var config = new TypeAdapterConfig();
            config.NewConfig<Person, PersonDto>()
                .Ignore(dest => dest.IrrelevantInformation)
                .Map(dest => dest.Name,
                    src => $"{src.FirstName} {src.LastName}");
            var mapper = new MapsterMapper.Mapper(config);
            for (var i = 0; i< 10000; i++) {
                sw.Start();
                var personDto = mapper.Map<PersonDto>(person);
                sw.Stop();
                time += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
            }
            Console.WriteLine("Mapster only mapping Elapsed={0}", time);

            sw.Reset();

            time = 0;
            var config2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDto>()
                    .ForMember(p => p.IrrelevantInformation, opt => opt.Ignore())
                    .ForMember(p => p.Name, opt => opt.MapFrom(s => string.Concat(s.FirstName, " ", s.LastName)))
                    .ForMember(p => p.SpouseAge, opt => opt.MapFrom(dto => dto.Spouse.Age))
                    .ForMember(p => p.SpouseName, opt => opt.MapFrom(dto => dto.Spouse.LastName));
            });
            IMapper mapper2 = new Mapper(config2);
            for (var i = 0; i< 10000; i++) {
                sw.Start();
                var personDto2 = mapper2.Map<PersonDto>(person);
                sw.Stop();
                time += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
            }
            Console.WriteLine("Automapper only mapping Elapsed={0}", time);
        }

        public void BenchmarkMappersOnlyMappingEasyMapping()
        {
            Stopwatch sw = new Stopwatch();
            double time = 0;
            var config = new TypeAdapterConfig();
            config.NewConfig<EasyPerson, EasyPersonDto>();
            var mapper = new MapsterMapper.Mapper(config);
            for (var i = 0; i< 10000; i++) {
                sw.Start();
                var personDto = mapper.Map<EasyPersonDto>(easyPerson);
                sw.Stop();
                time += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
            }
            Console.WriteLine("Mapster Easy Mapping only mapping Elapsed={0}", time);

            sw.Reset();

            time = 0;
            var config2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EasyPerson, EasyPersonDto>();
            });
            IMapper mapper2 = new Mapper(config2);
            for (var i = 0; i< 10000; i++) {
                sw.Start();
                var personDto2 = mapper2.Map<EasyPersonDto>(easyPerson);
                sw.Stop();
                time += sw.Elapsed.TotalMilliseconds;
                sw.Reset();
            }
            Console.WriteLine("Automapper Easy Mapping only mapping Elapsed={0}", time);
        }
    }
}
