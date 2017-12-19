using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using People.Data.Entities;
using People.Data.Enum;
using People.Data.Services;
using People.Data.ViewModel;

namespace People.API.Controllers
{
    // Note: any exception thrown from all these http methods will be handled at application level in "Startup.cs". 
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PetsController : Controller
    {
        private readonly ILogger<PetsController> _logger;
        private readonly IMapper _mapper;
        private readonly IPeopleRepository _repository;

        public PetsController(IPeopleRepository repository,
                              ILogger<PetsController> logger,
                              IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Pets?includePerson=true
        [HttpGet]
        public async Task<IActionResult> GetPetsAsync(bool includePerson = false)
        {
            var pets = await _repository.GetPetsAsync(true);

            var petsFromMalePerson = pets.Where(p => p.PersonPets.Any(o => o.Person.Gender == Gender.Male)).OrderBy(p => p.Name);
            var petsFromFemalePerson = pets.Where(p => p.PersonPets.Any(o => o.Person.Gender == Gender.Female)).OrderBy(p => p.Name);

            // group by person's gender.
            var petsViewModel = new PetsViewModel
            {
                PetsFromMaleOwner = petsFromMalePerson
                                    .Select(p => new PetViewModel
                                    {
                                        PetId = p.PetId,
                                        Name = p.Name,
                                        Type = p.Type.PetTypeName,
                                        People = includePerson ? p.PersonPets.Select(pp => _mapper.Map<Person, PersonViewModel>(pp.Person)).ToList() : null
                                    }).ToList(),
                PetsFromFemaleOwner = petsFromFemalePerson
                                    .Select(p => new PetViewModel
                                    {
                                        PetId = p.PetId,
                                        Name = p.Name,
                                        Type = p.Type.PetTypeName,
                                        People = includePerson ? p.PersonPets.Select(pp => _mapper.Map<Person, PersonViewModel>(pp.Person)).ToList() : null
                                    }).ToList(),
            };
            return Ok(petsViewModel);
        }

        // GET: api/Pets/5?includePerson=true
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetAsync([FromRoute] int id, bool includePerson = false)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pet = await _repository.GetPetAsync(id, includePerson);

            if(pet == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Pet, PetViewModel>(pet));
        }

    }


}