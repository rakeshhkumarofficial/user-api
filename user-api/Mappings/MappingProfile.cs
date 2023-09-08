using AutoMapper;
using user_api.Models;
using user_api.Models.DTOs;

namespace user_api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();

        }
    }
}
