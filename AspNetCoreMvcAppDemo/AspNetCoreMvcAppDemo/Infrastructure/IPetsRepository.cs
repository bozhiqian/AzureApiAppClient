using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Models;

namespace AspNetCoreMvcAppDemo.Infrastructure
{
    public interface IPetsRepository
    {
        Task<List<PetOwner>> GetPetOwnerAsync(string baseUrl, string requestUri);
    }
}