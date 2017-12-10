using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Infrastructure;
using AspNetCoreMvcAppDemo.Models;
using AspNetCoreMvcAppDemo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            var baseUrl = _configuration["API:BaseUrl"]; // "http://agl-developer-test.azurewebsites.net/"
            var requestUri = _configuration["API:RequestUri"]; // "people.json"

            try
            {
                var petOwners = await _petsRepository.GetPetOwnerAsync(baseUrl, requestUri);
                var petsFromMaleOwners = petOwners.Where(p => p.Gender == Gender.Male && p.Pets != null)
                                                  .SelectMany(p => p.Pets).OrderBy(p => p.Name).ToList();
                var petsFromFemaleOwners = petOwners.Where(p => p.Gender == Gender.Female).SelectMany(p => p.Pets)
                                                    .OrderBy(p => p.Name).ToList();
                var petsViewModel = new PetsViewModel
                {
                    PetsFromMaleOwner = petsFromMaleOwners,
                    PetsFromFemaleOwner = petsFromFemaleOwners
                };

                return View(petsViewModel);
            }
            catch(Exception ex)
            {
                _logger?.LogError(ex.ToString());
                throw ex;
            }

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
    }
}