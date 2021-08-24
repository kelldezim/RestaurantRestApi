using AutoMapper;
using RestaurantRestApi.DtoModels;
using RestaurantRestApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRestApi.MappingProfiles
{
    public class RestaurantMappingProfile : Profile
    {
        //if fields of objects has a same name they will be mapped automatically
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(r => r.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(r => r.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(r => r.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Dish, DishDto>();
            CreateMap<CreateDishDto, Dish>();
            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(restaurant => restaurant.Address,
                restAddress => restAddress.MapFrom(dto => new Address { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
        }
    }
}
