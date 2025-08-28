using AutoMapper;
using PMSApi.DTOs.UserDtos;
using PMSApi.Entities;
using System.Reflection;

namespace PMSApi.Profiles
{
    class UsersProfile : Profile
    {
        public UsersProfile()
        {
             CreateMap<User, UserReadDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.Firstname} {src.Lastname}")
                );
            CreateMap<UserLoginDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserRegisterDto, User>();
                   }

      
    }
}
