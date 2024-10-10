using Repository;
using Model;

namespace Service
{
    public class PersonService
    {
        private readonly IPersonDA personDA;
        public PersonService(IPersonDA personDA)
        {
            this.personDA = personDA;
        }
        public List<PersonDB> GetAllPeople()
        {
            return personDA.GetAll();
        }
        public PersonDB GetPersonById(int personId)
        {
            var person = personDA.GetById(personId);
            return person;
        }
        public PersonDB AddPerson(PersonDB person)
        {
            return personDA.Add(person);
        }
        public ExitCode UpdatePerson(PersonDB person)
        {
            return personDA.Update(person);
        }
        public ExitCode DeletePersonById(int personId)
        {
            return personDA.DeleteById(personId);
        }
    }
}