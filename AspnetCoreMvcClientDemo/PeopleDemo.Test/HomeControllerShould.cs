using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using People.Data.Model;
using People.Data.Services;
using People.Data.ViewModel;
using PeopleDemo.Controllers;
using Xunit;

namespace PeopleDemo.Test
{
    public class HomeControllerShould
    {
        private readonly Mock<IPetsRepository> _mockRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly List<PetOwner> _petOwners;
        
        public HomeControllerShould()
        {
            using(StreamReader r = new StreamReader("people.json"))
            {
                string json = r.ReadToEnd();
                _petOwners = JsonConvert.DeserializeObject<List<PetOwner>>(json);
            }

            _mockRepository = new Mock<IPetsRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            
        }
        
        [Fact]
        public async Task ValidatePetsFromOwners()
        {
            var petOwnersViewModel = new PetOwnersViewModel() {PetOwners = _petOwners};

            _mockRepository.Setup(x => x.BaseAddress).Returns("http://agl-developer-test.azurewebsites.net/");
            _mockRepository.Setup(x => x.RequestUri).Returns("people.json");
            _mockRepository.Setup(x => x.GetPetOwnerAsync()).ReturnsAsync(petOwnersViewModel);

            var sut = new HomeController(_mockConfiguration.Object, _mockRepository.Object, null);

            var result = await sut.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<PetsViewModels>(viewResult.Model);

            Assert.Equal(6, model.PetsViewModelAgl.PetsFromMaleOwner.Count);
            Assert.Equal(4, model.PetsViewModelAgl.PetsFromFemaleOwner.Count);
        }
    }
}
