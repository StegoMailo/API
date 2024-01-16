using AutoMapper;
using Usable_Security_Project_Key_Registry.Models;
using Usable_Security_Project_Key_Registry.Models.DTO;

namespace Usable_Security_Project_Key_Registry.Services.Mappers
{
    public class UserMapperProfile : Profile
    {

        public UserMapperProfile() {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
