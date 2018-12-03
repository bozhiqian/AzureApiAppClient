using System;
using System.Collections.Generic;
using System.Text;
using People.Data.Enum;

namespace People.Data.Model
{
    public class PetOwner
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public List<Pet> Pets { get; set; }

    }
}
