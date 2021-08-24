using RestaurantRestApi.DtoModels;
using System.Collections.Generic;

namespace RestaurantRestApi.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        List<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto createRestaurantDto);
        void Update(int id, UpdateRestaurantDto updateRestaurantDto);
        void Delete(int id);
    }
}
