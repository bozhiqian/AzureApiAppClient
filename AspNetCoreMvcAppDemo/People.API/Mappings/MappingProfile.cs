using AutoMapper;
using People.API.Data;
using People.Data.Entities;
using People.Data.ViewModel;

namespace People.API.Mappings
{
    public class PetMappingProfile : Profile
    {
        public PetMappingProfile()
        {
            CreateMap<Pet, PetViewModel>()
                .ForMember(o => o.Type, ex => ex.MapFrom(o => o.Type.PetTypeName));

            CreateMap<Person, PersonViewModel>()
                .ForMember(o => o.Gender, ex => ex.MapFrom(o => o.Gender.ToString()));
        }

    }
}
