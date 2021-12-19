using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.MappingTests
{
    class PersonDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public ICollection<Person> Relatives { get; set; }

        public string SpouseName { get; set; }
        public double SpouseAge { get; set; }
        public ICollection<string> IrrelevantInformation { get; set; }
    }
}
