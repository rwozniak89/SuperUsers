using AutoMapper;
using SuperUsers.Domain.Dtos;
using SuperUsers.Domain.Entities;

namespace SuperDevices.WebApi.Modules.AMProfiles
{
    public class UserPofiles : Profile
    {
        public UserPofiles()
        {
            //Source, Dest
            CreateMap<User, UserDto>();

            CreateMap<UserDto, User>()
                .ForMember(
                    dest => dest.CreditCardNumber,
                    opt => opt.Ignore());
        }
    }
}
