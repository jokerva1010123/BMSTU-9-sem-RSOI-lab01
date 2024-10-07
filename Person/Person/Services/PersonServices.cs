using Repository;
using Model;

namespace Service
{
    public class PersonService
    {
        private readonly IPersonDA _personDA;

        public PersonService(IPersonDA personDA)
        {
            _personDA = personDA;
        }

        public List<PersonDB> GetAllPeople()
        {
            return _personDA.FindAll();
        }

        public PersonDB GetPersonById(int personId)
        {
            var person = _personDA.FindById(personId);
            return person;
        }

        public PersonDB AddPerson(PersonDB person)
        {
            return _personDA.Add(person);
        }

        public ExitCode PatchPerson(PersonDB person)
        {
            return _personDA.Patch(person);
        }

        public ExitCode DeletePersonById(int personId)
        {
            return _personDA.DeleteById(personId);
        }
    }
}