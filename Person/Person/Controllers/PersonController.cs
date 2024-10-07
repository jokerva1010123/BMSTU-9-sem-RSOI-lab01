using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Controllers
{
    [ApiController]
    [Route("/api/v1/persons")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        private List<PersonDTO> ListPersonDTO(List<PersonDB> people)
        {
            var peopleDTO = new List<PersonDTO>();
            foreach (var person in people)
            {
                var personDTO = new PersonDTO(person);
                peopleDTO.Add(personDTO);
            }

            return peopleDTO;
        }

        /// <summary>Get all Persons</summary>
        /// <returns>People information</returns>
        /// <response code="200">All Persons</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PersonDTO>))]
        public IActionResult GetAllPeople()
        {
            List<PersonDB> people = _personService.GetAllPeople();
            List<PersonDTO> lPersonDTO = ListPersonDTO(people);
            return Ok(lPersonDTO);
        }

        /// <summary>Get Person by ID</summary>
        /// <returns>Person information</returns>
        /// <response code="200">Person for ID</response>
        /// <response code="404">Not found Person for ID</response>
        [HttpGet]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPersonById([FromRoute(Name = "Id")] int id)
        {
            var person = _personService.GetPersonById(id);
            if (person is null)
            {
                return NotFound();
            }

            var personDTO = new PersonDTO(person);
            return Ok(personDTO);
        }

        /// <summary>Create new Person</summary>
        /// <param name="personDTO">Person to add</param>
        /// <returns>Added person</returns>
        /// <response code="201">Created new Person</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPerson([FromBody] PersonDTO personDTO)
        {
            var person = personDTO.GetPerson();
            var result = _personService.AddPerson(person);

            if (result is null)
            {
                return BadRequest();
            }

            var header = $"api/v1/persons/{result.id}";
            return Created(header, person);
        }

        void FixedPatchFields(PersonDB personToPatch, PersonDB userPerson)
        {
            if (userPerson.name != null && userPerson.name != "string")
            {
                personToPatch.name = userPerson.name;
            }

            if (userPerson.address != null && userPerson.address != "string")
            {
                personToPatch.address = userPerson.address;
            }

            if (userPerson.work != null && userPerson.work != "string")
            {
                personToPatch.work = userPerson.work;
            }

            if (userPerson.age > 0)
            {
                personToPatch.age = userPerson.age;
            }
        }

        /// <summary>Update Person by ID</summary>
        /// <param name="personDTO">Person to update</param>
        /// <returns>Updated person</returns>
        /// <response code="200">Person for ID was updated</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Not found Person for ID</response>
        [HttpPatch]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchPerson([FromRoute(Name = "Id")] int id, [FromBody] PersonDTO personDTO)
        {
            var personToPatch = _personService.GetPersonById(id);
            if (personToPatch is null)
            {
                return NotFound();
            }

            var userPerson = personDTO.GetPerson(id);
            FixedPatchFields(personToPatch, userPerson);

            //Console.WriteLine("Id = " + personToPatch.id);
            //Console.WriteLine("Name = " + personToPatch.name);
            //Console.WriteLine("Age = " + personToPatch.age);
            //Console.WriteLine("Address = " + personToPatch.address);
            //Console.WriteLine("Work = " + personToPatch.work);
            

            ExitCode result = _personService.PatchPerson(personToPatch);
            if (result == ExitCode.Constraint)
            {
                return BadRequest();
            }

            var updatedPerson = new PersonDTO(personToPatch);
            return Ok(updatedPerson);
        }

        /// <summary>Remove Person by ID</summary>
        /// <returns>Removed person</returns>
        /// <response code="204">Person removed</response>
        [HttpDelete]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(PersonDTO))]
        public IActionResult DeletePerson([FromRoute(Name = "Id")] int id)
        {
            _personService.DeletePersonById(id);
            return NoContent();
        }
    }
}