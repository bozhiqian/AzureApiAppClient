using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Controllers;
using AspNetCoreMvcAppDemo.Infrastructure;
using AspNetCoreMvcAppDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using People.Data.ViewModel;
using Xunit;

namespace AspNetCoreMvcAppDemo.Test.Controller
{
    public class HomeControllerShould
    {
        public HomeControllerShould()
        {
            using(StreamReader r = new StreamReader("people.json"))
            {
                string json = r.ReadToEnd();
                _pets = JsonConvert.DeserializeObject<List<PetOwner>>(json);
            }

            using(StreamReader r = new StreamReader("people2.json"))
            {
                string json = r.ReadToEnd();
                _petsViewModel = JsonConvert.DeserializeObject<PetsViewModel>(json);
            }

            _mockRepository = new Mock<IPetsRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _sut = new HomeController(_mockConfiguration.Object, _mockRepository.Object, null);
        }

        private readonly Mock<IPetsRepository> _mockRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly HomeController _sut;
        private readonly List<PetOwner> _pets;
        private readonly PetsViewModel _petsViewModel;


        [Fact]
        public async Task ValidatePetsFromOwners()
        {
            _mockConfiguration.Setup(x => x["API:BaseUrl"]).Returns("http://agl-developer-test.azurewebsites.net/");
            _mockConfiguration.Setup(x => x["API:RequestUri"]).Returns("people.json");
            _mockRepository.Setup(x => x.GetPetOwnerAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_pets);

            _mockConfiguration.Setup(x => x["PeopleApi:BaseUrl"]).Returns("http://localhost:51888/");
            _mockConfiguration.Setup(x => x["PeopleApi:RequestUri"]).Returns("api/Pets");
            _mockRepository.Setup(x => x.GetPetsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_petsViewModel);

            var result = await _sut.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<PetsViewModels>(viewResult.Model);

            Assert.Equal(6, model.PetsViewModelAgl.PetsFromMaleOwner.Count);
            Assert.Equal(4, model.PetsViewModelAgl.PetsFromFemaleOwner.Count);
            Assert.Equal(6, model.PetsViewModelApi.PetsFromMaleOwner.Count);
            Assert.Equal(4, model.PetsViewModelApi.PetsFromFemaleOwner.Count);
        }
    }
}
