using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using People.API.Controllers;
using People.Data.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using People.Data.Entities;
using People.Data.Enum;
using People.Data.Services;
using Xunit;

namespace People.API.Test.Controller
{
    public class PetsControllerShould
    {
        private readonly Mock<IPeopleRepository> _mockRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly PetsController _sut;
        private readonly PetsViewModel _petsViewModel;
        private readonly List<Pet> _pets;
        private readonly List<PetType> _petTypes;

        public PetsControllerShould()
        {
            using(StreamReader r = new StreamReader("people2.json"))
            {
                string json = r.ReadToEnd();
                _petsViewModel = JsonConvert.DeserializeObject<PetsViewModel>(json);
                _petTypes = _petsViewModel.PetsFromMaleOwner.Select(p => p.Type)
                                             .Concat(_petsViewModel.PetsFromFemaleOwner.Select(p => p.Type)).Distinct()
                                             .Select(p => new PetType() { PetTypeName = p }).ToList();

                var petsMale = _petsViewModel.PetsFromMaleOwner.Select(p =>
                    new Pet()
                    {
                        PetId = p.PetId,
                        Name = p.Name,
                        Type = _petTypes.FirstOrDefault(pt => pt.PetTypeName == p.Type),
                        PersonPets = p.People.Select(ps => new PersonPet()
                        {
                            Person = new Person()
                            {
                                PersonId = ps.PersonId,
                                Name = ps.Name,
                                Gender = ps.Gender == "Male" ? Gender.Male : Gender.Female,
                                Age = ps.Age,
                            }
                        }).ToList()
                    }).ToList();

                var petsFemale = _petsViewModel.PetsFromFemaleOwner.Select(p =>
                    new Pet()
                    {
                        PetId = p.PetId,
                        Name = p.Name,
                        Type = _petTypes.FirstOrDefault(pt => pt.PetTypeName == p.Type),
                        PersonPets = p.People.Select(ps => new PersonPet()
                        {
                            Person = new Person()
                            {
                                PersonId = ps.PersonId,
                                Name = ps.Name,
                                Gender = ps.Gender == "Male" ? Gender.Male : Gender.Female,
                                Age = ps.Age,
                            }
                        }).ToList()
                    }).ToList();

                // Project all pets with distinct name.
                _pets = (from p in petsMale.Concat(petsFemale).Distinct().ToList()
                         group p by new { p.Name }
                                 into distinctNameGroup
                         select distinctNameGroup.First()).ToList();

            }

            _mockRepository = new Mock<IPeopleRepository>();

            //_mockConfiguration = new Mock<IConfiguration>();
            //_mockConfiguration.Setup(x => x["PeopleApi:BaseUrl"]).Returns("http://localhost:51888/");
            //_mockConfiguration.Setup(x => x["PeopleApi:RequestUri"]).Returns("api/Pets");

            _sut = new PetsController(_mockRepository.Object, null, null);
        }


        [Fact]
        public async Task ValidatePetsFromOwners()
        {
            _mockRepository.Setup(x => x.GetPetsAsync(It.IsAny<bool>())).ReturnsAsync(_pets);

            var actionResult = await _sut.GetPetsAsync();
            var value = ((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult).Value;
            var model = Assert.IsType<PetsViewModel>(value);

            Assert.NotNull(model);
            Assert.Equal(6, model.PetsFromMaleOwner.Count);
            Assert.Equal(4, model.PetsFromFemaleOwner.Count);


        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public async Task ValidateGivenPet(int petId)
        {
            _mockRepository.Setup(x => x.GetPetAsync(petId, It.IsAny<bool>())).ReturnsAsync(_pets.FirstOrDefault(p => p.PetId == petId));

            var actionResult = await _sut.GetPetAsync(petId, true);
            if(actionResult is NotFoundResult)
            {
                Assert.Null(_pets.FirstOrDefault(p => p.PetId == petId));
            }
            else
            {
                var value = ((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult).Value;
                var model = Assert.IsType<PetViewModel>(value);

                Assert.NotNull(model);
                Assert.Equal(2, model.People.Count);
            }
        }
    }
}

/*
 Testing Sample Pets data
 ================================

{
    "petsFromMaleOwner": [
        {
            "petId": 7,
            "name": "Fido",
            "petType": "Dog",
            "people": [
                {
                    "personId": 1,
                    "name": "Bob",
                    "gender": "Male",
                    "age": 23
                }
            ]
        },
        {
            "petId": 1,
            "name": "Garfield",
            "petType": "Cat",
            "people": [
                {
                    "personId": 1,
                    "name": "Bob",
                    "gender": "Male",
                    "age": 23
                },
                {
                    "personId": 2,
                    "name": "Jennifer",
                    "gender": "Female",
                    "age": 18
                }
            ]
        },
        {
            "petId": 4,
            "name": "Jim",
            "petType": "Cat",
            "people": [
                {
                    "personId": 4,
                    "name": "Fred",
                    "gender": "Male",
                    "age": 40
                }
            ]
        },
        {
            "petId": 3,
            "name": "Max",
            "petType": "Cat",
            "people": [
                {
                    "personId": 4,
                    "name": "Fred",
                    "gender": "Male",
                    "age": 40
                }
            ]
        },
        {
            "petId": 8,
            "name": "Sam",
            "petType": "Dog",
            "people": [
                {
                    "personId": 4,
                    "name": "Fred",
                    "gender": "Male",
                    "age": 40
                }
            ]
        },
        {
            "petId": 2,
            "name": "Tom",
            "petType": "Cat",
            "people": [
                {
                    "personId": 4,
                    "name": "Fred",
                    "gender": "Male",
                    "age": 40
                }
            ]
        }
    ],
    "petsFromFemaleOwner": [
        {
            "petId": 1,
            "name": "Garfield",
            "petType": "Cat",
            "people": [
                {
                    "personId": 1,
                    "name": "Bob",
                    "gender": "Male",
                    "age": 23
                },
                {
                    "personId": 2,
                    "name": "Jennifer",
                    "gender": "Female",
                    "age": 18
                }
            ]
        },
        {
            "petId": 9,
            "name": "Nemo",
            "petType": "Fish",
            "people": [
                {
                    "personId": 6,
                    "name": "Alice",
                    "gender": "Female",
                    "age": 64
                }
            ]
        },
        {
            "petId": 6,
            "name": "Simba",
            "petType": "Cat",
            "people": [
                {
                    "personId": 6,
                    "name": "Alice",
                    "gender": "Female",
                    "age": 64
                }
            ]
        },
        {
            "petId": 5,
            "name": "Tabby",
            "petType": "Cat",
            "people": [
                {
                    "personId": 5,
                    "name": "Samantha",
                    "gender": "Female",
                    "age": 40
                }
            ]
        }
    ]
}

*/
