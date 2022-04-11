using AutoMapper;
using dotnet_api_test.Exceptions.ExceptionResponses;
using dotnet_api_test.Models.Dtos;
using dotnet_api_test.Persistence.Repositories.Interfaces;
using dotnet_api_test.Validation;
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
            ReadDishDto dish = _mapper.Map<ReadDishDto>(_dishRepository.GetDishById(id));
            return Ok(dish);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<ReadDishDto> CreateDish([FromBody] CreateDishDto createDishDto)
        {
            Dish dish = _mapper.Map<Dish>(createDishDto);

            ModelValidation.ValidateCreateDishDto(createDishDto);
            ReadDishDto dishDto = _mapper.Map<ReadDishDto>(_dishRepository.CreateDish(dish));
            return Ok(dishDto);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ReadDishDto> UpdateDishById(int id, UpdateDishDto updateDishDto)
        {
            ModelValidation.ValidateUpdateDishDto(updateDishDto);

            Dish foundDish = _dishRepository.GetDishById(id);
            foundDish.Name = updateDishDto.Name;
            foundDish.MadeBy = updateDishDto.MadeBy;
            foundDish.Cost = (double) updateDishDto.Cost;

            _dishRepository.UpdateDish(foundDish);

            ReadDishDto dishDto = _mapper.Map<ReadDishDto>(foundDish);
            return Ok(dishDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDishById(int id)
        {
            Dish dish = _dishRepository.GetDishById(id);
            if (dish == null)
            {
                throw new NotFoundRequestExceptionResponse("No dish found with id: " + id, 404);
            }

            _dishRepository.DeleteDishById(id);
            return Ok("Deleted dish with id: " + id);
        }
    }
}