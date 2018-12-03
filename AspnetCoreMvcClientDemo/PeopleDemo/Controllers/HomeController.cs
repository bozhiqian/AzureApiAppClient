using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspnetCoreMvcClientDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using People.Data.Enum;
using People.Data.Model;
using People.Data.Services;
using People.Data.ViewModel;

namespace PeopleDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger = null;
        private readonly IPetsRepository _petsRepository;

        public HomeController(IConfiguration configuration, IPetsRepository petsRepository, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _petsRepository = petsRepository;
            _petsRepository.RequestUri = _configuration["API:RequestUri"]; // "people.json"
        }

        public async Task<IActionResult> Index()
        {
            #region Call AGL JSON Web service 

            var petsViewModelFromAgl = await GetPetsViewModelFromAglAsync();

            #endregion

            return View(new PetsViewModels
            {
                PetsViewModelAgl = petsViewModelFromAgl,
                AglJsonUrl = new Uri(_petsRepository.BaseAddress + _petsRepository.RequestUri),
            });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<PetsViewModel> GetPetsViewModelFromAglAsync()
        {
            #region Call AGL JSON Web service 

            var petOwnersViewModel = await _petsRepository.GetPetOwnerAsync();

            if (petOwnersViewModel.Exception != null)
            {
                _logger?.LogError(petOwnersViewModel.Exception.ToString());
                throw petOwnersViewModel.Exception;
            }

            var petOwners = petOwnersViewModel.PetOwners;

            var petsFromMaleOwners = petOwners.Where(p => p.Gender == Gender.Male && p.Pets != null)
                .SelectMany(p => p.Pets).OrderBy(p => p.Name)
                .Select(p => new Pet { Name = p.Name, Type = p.Type.ToString() }).ToList();
            var petsFromFemaleOwners = petOwners.Where(p => p.Gender == Gender.Female).SelectMany(p => p.Pets)
                .OrderBy(p => p.Name)
                .Select(p => new Pet { Name = p.Name, Type = p.Type.ToString() }).ToList();

            var petsViewModel = new PetsViewModel
            {
                PetsFromMaleOwner = petsFromMaleOwners,
                PetsFromFemaleOwner = petsFromFemaleOwners,
                StatusCode = petOwnersViewModel.StatusCode
            };

            return petsViewModel;

            #endregion
        }

    }
}