using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Models;

namespace AspNetCoreMvcAppDemo.ViewModel
{
    public class PetsViewModel
    {
        public List<Pet> PetsFromMaleOwner { get; set; }
        public List<Pet> PetsFromFemaleOwner { get; set; }
    }
}
