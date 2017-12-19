using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using People.Data.Entities;

namespace People.Data.Services
{
    public interface IPeopleRepository
    {
        IQueryable<Person> GetPeople(bool includePets = false);
        Person GetPerson(int id, bool includePets = false);
        IQueryable<Pet> GetPets(bool includePerson = false);
        Task<List<Pet>> GetPetsAsync(bool includePerson = false);
        Task<Pet> GetPetAsync(int id, bool includePerson = false);
    }
}