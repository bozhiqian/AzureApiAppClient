using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using People.Data.Entities;

namespace People.Data.Services
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly PeopleContext _context;
        private readonly ILogger<PeopleRepository> _logger;
        public PeopleRepository(PeopleContext context, ILogger<PeopleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Person> GetPeople(bool includePets = false)
        {
            try
            {
                if(includePets)
                {
                    return _context.People.Include(p => p.PersonPets);
                }
                return _context.People;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get people: {ex}");
                throw;
            }

        }

        public Person GetPerson(int id, bool includePets = false)
        {
            try
            {
                if(includePets)
                {
                    return _context.People.Include(p => p.PersonPets).FirstOrDefault(p => p.PersonId == id);
                }
                return _context.People.FirstOrDefault(p => p.PersonId == id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get the person: {ex}");
                throw;
            }
        }

        public IQueryable<Pet> GetPets(bool includePerson = false)
        {
            try
            {
                if(includePerson)
                {
                    return _context.Pets.Include(p => p.PersonPets).ThenInclude(p => p.Person).Include(p => p.Type);
                }
                return _context.Pets;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get pets: {ex}");
                throw;
            }

        }

        public async Task<List<Pet>> GetPetsAsync(bool includePerson = false)
        {
           return await GetPets(includePerson).ToListAsync();
        }

        public async Task<Pet> GetPetAsync(int id, bool includePerson = false)
        {
            try
            {
                if(includePerson)
                {
                    return await _context.Pets.Include(p => p.PersonPets).ThenInclude(p => p.Person).Include(p => p.Type).FirstOrDefaultAsync(p => p.PetId == id);
                }
                return await _context.Pets.FirstOrDefaultAsync(p => p.PetId == id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get the pet: {ex}");
                throw;
            }
        }
    }
}
