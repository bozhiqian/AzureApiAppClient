using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using People.Data.Entities;

namespace People.API.Data
{
    public class PeopleSeeder
    {
        private readonly PeopleContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly IMapper _mapper;
        public PeopleSeeder(PeopleContext context, IHostingEnvironment hosting, IMapper mapper)
        {
            _context = context;
            _hosting = hosting;
            _mapper = mapper;
        }

        public void Seed()
        {
            // _context.Database.EnsureDeleted(); // Delete database, debugging use only. 
            _context.Database.EnsureCreated();

            if(!_context.People.Any())
            {
                // Need to create sample data
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/people.json");
                var json = File.ReadAllText(filepath);
                var personModels = JsonConvert.DeserializeObject<List<PersonModel>>(json);
                var petsWithDuplicates = personModels.Where(p => p.Pets != null).SelectMany(m => m.Pets).Distinct().ToList();

                var petModels = (from p in petsWithDuplicates
                                 group p by new { p.Name }
                                 into distinctNameGroup
                                 select distinctNameGroup.First()).ToList();

                var petTypes = petsWithDuplicates.Select(p => p.Type).Distinct().OrderBy(p => p).Select(p => new PetType { PetTypeName = p }).ToList();
                _context.PetTypes.AddRange(petTypes);

                var pets = petModels.Select(pm =>
                    new Pet { Name = pm.Name, Type = petTypes.FirstOrDefault(pt => pt.PetTypeName.ToLower() == pm.Type.ToLower()) }).ToList(); 
                _context.Pets.AddRange(pets);


                var people = personModels.Select(p => new Person
                {
                    PersonId = p.PersonId,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    PersonPets = p.Pets?.Select(pm => new PersonPet
                    {
                        PersonId = p.PersonId,
                        Pet = pets.FirstOrDefault(pet => pet.Name == pm.Name && pet.Type.PetTypeName.ToString().ToLower() == pm.Type.ToLower()) 
                    }).ToList() ?? new List<PersonPet>()
                }).ToList();

                _context.People.AddRange(people);

                _context.SaveChanges();

            }
        }
    }
}

