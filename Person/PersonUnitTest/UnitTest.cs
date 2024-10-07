using Model;
using Repository;
using Moq;
using Service;

namespace PeopleTests
{
    public class UnitTests
    {
        [Xunit.Fact]
        public void FindAll_NotNull()
        {
            // Arrange
            var expPerson = new PersonBuilder().Build();
            var expPeople = new List<PersonDB>() { expPerson };

            var mock = new Mock<IPersonDA>();
            mock.Setup(x => x.FindAll())
                .Returns(expPeople);
            var personService = new PersonService(mock.Object);

            // Act
            var actPeople = personService.GetAllPeople();

            // Assert
            Assert.NotNull(expPeople);
            Assert.Equal(expPeople, actPeople);
        }

        [Fact]
        public void FindById_FirstElement_NotNull()
        {
            const int personId = 1;

            // Arrange
            var expPerson = new PersonBuilder()
                .WherePersonId(personId)
                .Build();

            var mock = new Mock<IPersonDA>();
            mock.Setup(x => x.FindById(personId))
                .Returns(expPerson);
            var personService = new PersonService(mock.Object);

            // Act
            var actPerson = personService.GetPersonById(personId);

            // Assert
            Assert.NotNull(expPerson);
            Assert.Equal(expPerson, actPerson);
        }

        [Fact]
        public void AddPerson_Ok()
        {
            // Arrange
            var accessObject = new PeopleAccessObject();
            var personToAdd = CreatePerson();
            AddEntity(accessObject, personToAdd);

            // Act
            accessObject.personRepository.Add(personToAdd);

            // Assert
            var addedPerson = accessObject.personRepository.FindById(personToAdd.id);

            Assert.NotNull(addedPerson);
            Assert.Equal(personToAdd, addedPerson);

            Cleanup(accessObject);
        }

        [Fact]
        public void UpdatePerson_Ok()
        {
            // Arrange
            var accessObject = new PeopleAccessObject();
            var personToUpdate = CreatePerson();
            AddEntity(accessObject, personToUpdate);

            // Act
            personToUpdate.age += 5;
            accessObject.personRepository.Patch(personToUpdate);

            // Assert
            var updatedPerson = accessObject.personRepository.FindById(personToUpdate.id);
            Assert.NotNull(updatedPerson);
            Assert.Equal(personToUpdate, updatedPerson);

            Cleanup(accessObject);
        }

        [Fact]
        public void DeletePersonById_Ok()
        {
            // Arrange
            var accessObject = new PeopleAccessObject();
            var personToDelete = CreatePerson();
            AddEntity(accessObject, personToDelete);

            // Act
            var id = personToDelete.id;
            accessObject.personRepository.DeleteById(id);

            // Assert
            var removedPerson = accessObject.personRepository.FindById(id);
            Assert.Null(removedPerson);

            Cleanup(accessObject);
        }

        PersonDB CreatePerson()
        {
            var person = new PersonDB()
            {
                id = 1,
                name = "Test",
                age = 1,
                address = "Address",
                work = "Pass"
            };
            return person;
        }

        void AddEntity(PeopleAccessObject accessObject, PersonDB person)
        {
            accessObject.peopleContext.ChangeTracker.Clear();
            accessObject.peopleContext.People.Add(person);
            accessObject.peopleContext.SaveChanges();
        }

        void Cleanup(PeopleAccessObject accessObject)
        {
            accessObject.peopleContext.ChangeTracker.Clear();
            accessObject.peopleContext.People.RemoveRange(accessObject.peopleContext.People);
            accessObject.peopleContext.SaveChanges();
        }
    }
}