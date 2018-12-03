using System.Collections.Generic;
using System.Threading.Tasks;
using People.Data.Model;
using People.Data.ViewModel;

namespace People.Data.Services
{
    public interface IPetsRepository
    {
        Task<PetOwnersViewModel> GetPetOwnerAsync();
        string RequestUri { get; set; }
        string BaseAddress { get; }
    }
}