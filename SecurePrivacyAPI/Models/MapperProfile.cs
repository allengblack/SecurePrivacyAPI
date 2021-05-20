using AutoMapper;

namespace SecurePrivacyAPI.Models
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<PersonDto, Person>().ReverseMap()
                .ForMember(dest => dest.Address, opt => opt.Condition(c => c.Address != null))
                .ForMember(dest => dest.Age, opt => opt.Condition(c => c.Age is not 0)); //.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember is not null and not 0));
        }
    }
}
