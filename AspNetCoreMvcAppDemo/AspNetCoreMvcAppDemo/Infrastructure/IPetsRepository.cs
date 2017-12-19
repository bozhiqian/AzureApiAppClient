using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Models;
using People.Data.ViewModel;

namespace AspNetCoreMvcAppDemo.Infrastructure
{
    public interface IPetsRepository
    {
        Task<List<PetOwner>> GetPetOwnerAsync(string baseUrl, string requestUri);
        Task<PetsViewModel> GetPetsAsync(string baseUrl, string requestUri);
    }
}