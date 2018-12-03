using System.Collections.Generic;
using System.Net;
using People.Data.Model;

namespace People.Data.ViewModel
{
    public class PetsViewModel//: ViewModel<PetsViewModel> 
    {
        public List<Pet> PetsFromMaleOwner { get; set; }
        public List<Pet> PetsFromFemaleOwner { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
