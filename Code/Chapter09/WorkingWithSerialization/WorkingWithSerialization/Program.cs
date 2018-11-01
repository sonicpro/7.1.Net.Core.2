using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using static System.Console;
using static System.Environment;
using static System.IO.Path;

namespace Packt.CS7
{
    class Program
    {
        static void Main(string[] args)
        {
            var people = new List<Person>
            {
                new Person(30000M) { FirstName = "Alice", LastName = "Smith", DateOfBirth = new DateTime(1974, 3, 14) },
                new Person(40000M) { FirstName = "Bob", LastName = "Jones", DateOfBirth = new DateTime(1969, 11, 23) },
                new Person(20000.12M)
                {
                    FirstName = "Charlie",
                    LastName = "Rose", DateOfBirth = new DateTime(1964, 5, 4),
                    Children = new HashSet<Person> { new Person(0M) { FirstName = "Sally", LastName = "Rose", DateOfBirth = new DateTime(1990, 7, 12) } }
                }
            };

            string path = Combine(CurrentDirectory, "people.xml");

            FileStream stream = File.Create(path);

            // XML formatter for List<Person> objects to XML and for getting and object with the List<Person> structure back.
            var xs = new XmlSerializer(typeof(List<Person>));
            xs.Serialize(stream, people);
            stream.Close();

            WriteLine($"Written {new FileInfo(path).Length} bytes of XML to {path}");
            WriteLine();

            // Display the serialized object graph.
            WriteLine(File.ReadAllText(path));

            FileStream xmlLoad = File.Open(path, FileMode.Open);

            var loadedPeople = (List<Person>)xs.Deserialize(xmlLoad);

            foreach(Person p in loadedPeople)
            {
                WriteLine($"{p.LastName} has {p.Children.Count} children.");
            }

            if (loadedPeople.All(p => p.HasZeroSalary))
            {
                WriteLine("Salary data is totally lost during the serialization");
            }
        }
    }
}
