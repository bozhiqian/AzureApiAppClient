using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Infrastructure;
using AspNetCoreMvcAppDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using People.Data.Enum;
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

            var petsViewModelFromAgl = await GetPetsViewModelFromAGLAsync(baseUrl, requestUri);

            #endregion

            #region Call PeopleAPI web api
            var peopleBaseUrl = _configuration["PeopleApi:BaseUrl"]; // "http://localhost:51888/"
            var peopleRequestUri = _configuration["PeopleApi:RequestUri"]; // "api/Pets"
            var petsViewModel = await GetPetsViewModelAsync(peopleBaseUrl, peopleRequestUri);

            #endregion

            return View(new PetsViewModels
            {
                PetsViewModelAgl = petsViewModelFromAgl,
                PetsViewModelApi = petsViewModel,
                AglJsonUrl = new Uri(baseUrl + requestUri),
                PeopleApiUrl = new Uri(peopleBaseUrl + peopleRequestUri)
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

        private async Task<PetsViewModel> GetPetsViewModelFromAGLAsync(string baseUrl, string requestUri)
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

        private async Task<PetsViewModel> GetPetsViewModelAsync(string baseUrl, string requestUri)
        {
            #region Call PeopleAPI web api

            try
            {
                var petsViewModel = await _petsRepository.GetPetsAsync(baseUrl, requestUri);

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