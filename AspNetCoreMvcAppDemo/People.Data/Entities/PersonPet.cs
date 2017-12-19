using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace People.Data.Entities
{
    public class PersonPet
    {
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        public int PetId { get; set; }
        [ForeignKey("PetId")]
        public Pet Pet { get; set; }
    }
}
