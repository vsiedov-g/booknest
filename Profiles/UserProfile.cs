using System;
using AutoMapper;
using booknest.Models;
using booknest.Models.DTO;

namespace booknest.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
