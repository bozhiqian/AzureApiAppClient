using System.Collections.Generic;
using People.Data.Entities;

namespace People.Data.ViewModel
{
    public class PetViewModel
    {
        public int PetId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public List<PersonViewModel> People { get; set; }
    }
}
