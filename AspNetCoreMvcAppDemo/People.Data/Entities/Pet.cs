using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace People.Data.Entities
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public PetType Type { get; set; }

        public ICollection<PersonPet> PersonPets { get; set; } = new List<PersonPet>();
    }
}
