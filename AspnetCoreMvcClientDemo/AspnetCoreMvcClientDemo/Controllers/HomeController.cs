using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspnetCoreMvcClientDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using People.Data.Enum;
using People.Data.Services;
using People.Data.ViewModel;

namespace AspNetCoreMvcAppDemo.Controllers
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
        }

        public async Task<IActionResult> Index()
        {
            #region Call AGL JSON Web service 
            var baseUrl = _configuration["API:BaseUrl"]; // "http://agl-developer-test.azurewebsites.net/"
            var requestUri = _configuration["API:RequestUri"]; // "people.json"

            var petsViewModelFromAgl = await GetPetsViewModelFromAglAsync(baseUrl, requestUri);

            #endregion

            return View(new PetsViewModels
            {
                PetsViewModelAgl = petsViewModelFromAgl,
                AglJsonUrl = new Uri(baseUrl + requestUri),
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

        private async Task<PetsViewModel> GetPetsViewModelFromAglAsync(string baseUrl, string requestUri)
        {
            #region Call AGL JSON Web service 

            try
            {
                var petOwners = await _petsRepository.GetPetOwnerAsync(baseUrl, requestUri);
                var petsFromMaleOwners = petOwners.Where(p => p.Gender == Gender.Male && p.Pets != null)
                                                  .SelectMany(p => p.Pets).OrderBy(p => p.Name)
                                                  .Select(p => new PetViewModel { Name = p.Name, Type = p.Type.ToString() }).ToList();
                var petsFromFemaleOwners = petOwners.Where(p => p.Gender == Gender.Female).SelectMany(p => p.Pets)
                                                    .OrderBy(p => p.Name)
                                                    .Select(p => new PetViewModel { Name = p.Name, Type = p.Type.ToString() }).ToList();
                var petsViewModel = new PetsViewModel
                {
                    PetsFromMaleOwner = petsFromMaleOwners,
                    PetsFromFemaleOwner = petsFromFemaleOwners
                };

                return petsViewModel;
            }
            catch(Exception ex)
            {
                _logger?.LogError(ex.ToString());
                throw ex;
            }
            #endregion
        }

    }
}