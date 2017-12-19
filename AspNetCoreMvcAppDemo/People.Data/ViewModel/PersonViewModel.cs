using System;
using System.Collections.Generic;
using System.Text;
using People.Data.Entities;
using People.Data.Enum;

namespace People.Data.ViewModel
{
    public class PersonViewModel
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
