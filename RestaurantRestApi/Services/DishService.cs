using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantRestApi.DtoModels;
using RestaurantRestApi.Entities;
using RestaurantRestApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRestApi.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext restaurantDbContext, IMapper mapper)
        {
           _context = restaurantDbContext;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDto createDishDto)
        {
            var restaurant = GetRestaurantByIdWithDishes(restaurantId);
            var dish = _mapper.Map<Dish>(createDishDto);
            dish.RestaurantId = restaurantId;

           _context.Dishes.Add(dish);
           _context.SaveChanges();

            return dish.Id;
        }
        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantByIdWithDishes(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish with such a Id does not exist in provided restaurant");
            }

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }
        public List<DishDto> Get(int restaurantId)
        {
            var restaurant = GetRestaurantByIdWithDishes(restaurantId);
            var dishesDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishesDto;
        }

        public void DeleteById(int restaurantId, int dishId)
        {
           var restaurant =  GetRestaurantByIdWithDishes(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish == null)
            {
                throw new NotFoundException("We cannot delete dish with such a Id because does not exist in provided restaurant");
            }

            restaurant.Dishes.Remove(dish);
           _context.SaveChanges();
        }
        private Restaurant GetRestaurantByIdWithDishes(int restaurantId)
        {
            var restaurant =_context.Restaurants
                                .Include(d => d.Dishes)
                                .FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant with such a Id does not exist");
            }

            return restaurant;
        }

        public void DeleteAll(int restaurantId)
        {
            var restaurant = GetRestaurantByIdWithDishes(restaurantId);
           _context.RemoveRange(restaurant.Dishes);
           _context.SaveChanges();
        }
    }
}
