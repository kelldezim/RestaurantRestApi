using AutoMapper;
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
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public Mapper Mapper { get; }
        public IMapper Mapper1 { get; }

        public RestaurantDto GetById (int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }
        public List<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();

            if (_dbContext == null) return null;

            var result = _mapper.Map<List<RestaurantDto>>(restaurants);

            return result;
        }
        public int Create(CreateRestaurantDto createRestaurantDto)
        {
            var restaurant = _mapper.Map<Restaurant>(createRestaurantDto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked");
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r =>r.Id == id);

            if(restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }
            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void Update(int id, UpdateRestaurantDto updateRestaurantDto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            restaurant.Name = updateRestaurantDto.Name;
            restaurant.Description = updateRestaurantDto.Description;
            restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}
