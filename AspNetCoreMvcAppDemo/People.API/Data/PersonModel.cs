using System.Collections.Generic;
using People.Data.Enum;

namespace People.API.Data
{
    public class PersonModel
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }
        public int Age { get; set; }
        public ICollection<PetModel> Pets { get; set; } = new List<PetModel>();
    }
}
