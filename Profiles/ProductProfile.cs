using System;
using AutoMapper;
using booknest.Models;
using booknest.Models.DTO;

namespace booknest.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ForMember(
                dest => dest.Author, src => src.MapFrom(x => x.Author.Name))
                .ForMember(
                    dest => dest.Categories, src => src.MapFrom(x => x.Categories.Select(c => c.Name).ToArray())
                );
        }
    }
}
