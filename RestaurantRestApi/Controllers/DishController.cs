using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantRestApi.DtoModels;
using RestaurantRestApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRestApi.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto createDishDto)
        {
            var dishId =_dishService.Create(restaurantId, createDishDto);

            return Created($"api/restaurant/{restaurantId}/dish/{dishId}", null);
        }
        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dishDto = _dishService.GetById(restaurantId, dishId);

            return Ok(dishDto);
        }
        [HttpGet]
        public ActionResult Get(int restaurantId)
        {
            List<DishDto> dishes =_dishService.Get(restaurantId);

            return Ok(dishes);
        }
        [HttpDelete("{dishId}")]
        public ActionResult Delete(int restaurantId, int dishId)
        {
            _dishService.DeleteById(restaurantId, dishId);

            return NoContent();
        }
        [HttpDelete]
        public ActionResult Delete(int restaurantId)
        {
            _dishService.DeleteAll(restaurantId);

            return NoContent();
        }
    }
}
