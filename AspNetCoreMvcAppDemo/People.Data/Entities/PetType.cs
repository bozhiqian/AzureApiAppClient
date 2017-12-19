using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace People.Data.Entities
{
    public class PetType
    {
        [Key]
        public int PetTypeId { get; set; }

        [Required]
        public string PetTypeName { get; set; }
    }
}
