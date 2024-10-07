using System;
using Model;

namespace PeopleTests
{
    public class PersonBuilder
    {
        private int Id;
        private string Name;
        private int Age;
        private string Address;
        private string Work;

        public PersonBuilder()
        {
            Id = 0;
            Name = string.Empty;
            Age = 0;
            Address = string.Empty;
            Work = string.Empty;
        }

        public PersonDB Build()
        {
            var person = new PersonDB()
            {
                id = Id,
                name = Name,
                age = Age,
                address = Address,
                work = Work
            };

            return person;
        }

        public PersonBuilder WherePersonId(int personId)
        {
            Id = personId;
            return this;
        }

        public PersonBuilder WhereAge(int age)
        {
            Age = age;
            return this;
        }
    }
}