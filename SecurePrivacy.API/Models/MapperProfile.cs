using AutoMapper;

namespace SecurePrivacy.API.Models
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<PersonDto, Person>().ReverseMap();
        }
    }
}