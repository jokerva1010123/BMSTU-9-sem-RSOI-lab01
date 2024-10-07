namespace Model
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Work { get; set; }

        public PersonDTO() 
        {
            Id = 0;
            Name = "string";
            Age = 0;
            Address = "string";
            Work = "string";
        }

        public PersonDTO(PersonDB person)
        {
            Id = person.id;
            Name = person.name;
            Age = person.age;
            Address = person.address;
            Work = person.work;
        }

        public PersonDB GetPerson(int personId = 0)
        {
            var person = new PersonDB()
            {
                id = personId,
                name = Name,
                age = Age,
                address = Address,
                work = Work
            };

            return person;
        }
    }
}