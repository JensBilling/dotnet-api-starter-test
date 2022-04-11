using AutoMapper;
using dotnet_api_test.Models.Dtos;
using dotnet_api_test.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_api_test.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;

        public DishController(ILogger<DishController> logger, IMapper mapper, IDishRepository dishRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _dishRepository = dishRepository;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<DishesAndAveragePriceDto> GetDishesAndAverageDishPrice()
        {
            DishesAndAveragePriceDto dishesAndAveragePriceDto = new DishesAndAveragePriceDto
            {
                Dishes = _mapper.Map<IEnumerable<ReadDishDto>>(_dishRepository.GetAllDishes()),
                AveragePrice = _dishRepository.GetAverageDishPrice()
            };
            return Ok(dishesAndAveragePriceDto);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ReadDishDto> GetDishById(int id)
        {
            Dish dish = _dishRepository.GetDishById(id);
            return Ok(dish);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<ReadDishDto> CreateDish([FromBody] CreateDishDto createDishDto)
        {
            Dish dish = _dishRepository.CreateDish(_mapper.Map<Dish>(createDishDto));
            return Ok(dish);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ReadDishDto> UpdateDishById(int id, UpdateDishDto updateDishDto)
        {
            Dish dish = _dishRepository.GetDishById(id);
            dish.Name = updateDishDto.Name;
            dish.MadeBy = updateDishDto.MadeBy;
            dish.Cost = (double) updateDishDto.Cost;

            _dishRepository.UpdateDish(dish);
            
            return Ok(dish);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDishById(int id)
        {
            _dishRepository.DeleteDishById(id);
            return Ok("Deleted dish with id: " + id);
        }
    }
}