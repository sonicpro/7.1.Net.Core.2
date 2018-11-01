using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Packt.CS7
{
    public class Person
    {
        public Person () { }

        public Person(decimal initialSalary)
        {
            Salary = initialSalary;
        }

        [XmlAttribute("fname")]
        public string FirstName { get; set; }

        [XmlAttribute("lname")]
        public string LastName { get; set; }

        [XmlAttribute("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        public HashSet<Person> Children { get; set; }

        public bool HasZeroSalary => Salary == 0;

        // Not public, won't be serialized.
        protected decimal Salary { get; set; }
    }
}
