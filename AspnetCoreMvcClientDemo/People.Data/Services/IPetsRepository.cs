using System.Collections.Generic;
using System.Threading.Tasks;
using People.Data.Model;
using People.Data.ViewModel;

namespace People.Data.Services
{
    public interface IPetsRepository
    {
        Task<List<PetOwner>> GetPetOwnerAsync(string baseUrl, string requestUri);
        Task<PetsViewModel> GetPetsAsync(string baseUrl, string requestUri);
    }
}