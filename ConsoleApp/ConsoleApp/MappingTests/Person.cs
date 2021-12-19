using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.MappingTests
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public ICollection<Person> Relatives { get; set; }

        public Person Spouse { get; set; }

        public ICollection<string> IrrelevantInformation { get; set; }
    }
}
