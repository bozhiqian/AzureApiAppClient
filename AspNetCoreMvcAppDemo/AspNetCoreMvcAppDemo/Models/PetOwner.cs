using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using People.Data.Entities;
using People.Data.Enum;
using People.Data.ViewModel;

namespace AspNetCoreMvcAppDemo.Models
{
   public class PetOwner
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public List<PetViewModel> Pets { get; set; }

    }
}
