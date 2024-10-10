using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Repository;

namespace Controllers
{
    [ApiController]
    [Route("/api/v1/persons")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService personService;
        public PersonController(PersonService personService)
        {
            this.personService = personService;
        }
        private List<PersonDTO> AllPersonDTO(List<PersonDB> people)
        {
            var peopleDTO = new List<PersonDTO>();
            foreach (var person in people)
            {
                var personDTO = new PersonDTO(person);
                peopleDTO.Add(personDTO);
            }
            return peopleDTO;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PersonDTO>))]
        public IActionResult GetAllPeople()
        {
            List<PersonDB> people = personService.GetAllPeople();
            List<PersonDTO> lPersonDTO = AllPersonDTO(people);
            return Ok(lPersonDTO);
        }
        [HttpGet]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPersonById([FromRoute(Name = "Id")] int id)
        {
            var person = personService.GetPersonById(id);
            if (person is null)
                return NotFound();
            var personDTO = new PersonDTO(person);
            return Ok(personDTO);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPerson([FromBody] PersonDTO personDTO)
        {
            var person = personDTO.GetPerson();
            var result = personService.AddPerson(person);
            if (result is null)
                return BadRequest();
            var header = $"api/v1/persons/{result.id}";
            return Created(header, person);
        }
        void FixedPatchFields(PersonDB personToPatch, PersonDB userPerson)
        {
            if (userPerson.name != null && userPerson.name != "string")
                personToPatch.name = userPerson.name;
            if (userPerson.address != null && userPerson.address != "string")
                personToPatch.address = userPerson.address;
            if (userPerson.work != null && userPerson.work != "string")
                personToPatch.work = userPerson.work;
            if (userPerson.age > 0)
                personToPatch.age = userPerson.age;
        }
        [HttpPatch]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePerson([FromRoute(Name = "Id")] int id, [FromBody] PersonDTO personDTO)
        {
            var personToPatch = personService.GetPersonById(id);
            if (personToPatch is null)
                return NotFound();
            var userPerson = personDTO.GetPerson(id);
            FixedPatchFields(personToPatch, userPerson);
            ExitCode result = personService.UpdatePerson(personToPatch);
            if (result == ExitCode.Constraint)
                return BadRequest();
            var updatedPerson = new PersonDTO(personToPatch);
            return Ok(updatedPerson);
        }
        [HttpDelete]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(PersonDTO))]
        public IActionResult DeletePerson([FromRoute(Name = "Id")] int id)
        {
            personService.DeletePersonById(id);
            return NoContent();
        }
    }
}