using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using People.Data.Enum;

namespace People.Data.Entities
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public ICollection<PersonPet> PersonPets { get; set; } = new List<PersonPet>();
    }
}
