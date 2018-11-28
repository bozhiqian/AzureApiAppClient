using System.Collections.Generic;

namespace People.Data.ViewModel
{
    public class PetsViewModel
    {
        public List<PetViewModel> PetsFromMaleOwner { get; set; }
        public List<PetViewModel> PetsFromFemaleOwner { get; set; }
    }
}
