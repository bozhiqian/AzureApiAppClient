using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Infrastructure;
using AspNetCoreMvcAppDemo.Models;
using AspNetCoreMvcAppDemo.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcAppDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPetsRepository _petsRepository;

        public HomeController(IPetsRepository petsRepository)
        {
            _petsRepository = petsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var petOwners = await _petsRepository.GetPetOwnerAsync("http://agl-developer-test.azurewebsites.net/", "people.json");
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
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}