using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRestApi.DtoModels;
using RestaurantRestApi.Entities;
using RestaurantRestApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRestApi.Controllers
{
    [Route("api/restaurant")]
    [ApiController] // it's running ModelState.IsValid ad throw bad request if not
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        [HttpPost]
        public ActionResult<RestaurantDto> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto)
        {
            int createdRestaurantId = _restaurantService.Create(createRestaurantDto);

            return Created($"api/restaurant/{createdRestaurantId}", null);
        }
        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantsDto = _restaurantService.GetAll();

            return Ok(restaurantsDto);

        }
        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {

            var restaurantDto = _restaurantService.GetById(id);

            return Ok(restaurantDto);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateRestaurantDto updateRestaurantDto, [FromRoute] int id)
        {
            _restaurantService.Update(id, updateRestaurantDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Remove([FromRoute]int id)
        {
            _restaurantService.Delete(id);

            return NotFound();
        }
    }
}
