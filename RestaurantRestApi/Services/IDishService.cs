using RestaurantRestApi.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRestApi.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto createDishDto);
        DishDto GetById(int restaurantId, int dishId);
        List<DishDto> Get(int restaurantId);
        void DeleteById(int restaurantId, int dishId);
        void DeleteAll(int restaurantId);
    }
}
