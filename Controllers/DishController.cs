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

            _logger.LogInformation("Sent all dishes from database + average price as response.");
            return Ok(dishesAndAveragePriceDto);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ReadDishDto> GetDishById(int id)
        {
            ReadDishDto dish = _mapper.Map<ReadDishDto>(_dishRepository.GetDishById(id));

            _logger.LogInformation("Sent dish with id: " + id + " as response.");
            return Ok(dish);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<ReadDishDto> CreateDish([FromBody] CreateDishDto createDishDto)
        {
            Dish dish = _mapper.Map<Dish>(createDishDto);

            ModelValidation.ValidateCreateDishDto(createDishDto);
            ValidateNoConflictInDatabase.ValidateCreateUniqueDishName(createDishDto.Name, _dishRepository.GetAllDishes());
            ReadDishDto dishDto = _mapper.Map<ReadDishDto>(_dishRepository.CreateDish(dish));

            _logger.LogInformation("New dish created and saved to database");
            return Ok(dishDto);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ReadDishDto> UpdateDishById(int id, UpdateDishDto updateDishDto)
        {
            ModelValidation.ValidateUpdateDishDto(updateDishDto);

            Dish foundDish = _dishRepository.GetDishById(id);
            ValidateNoConflictInDatabase.ValidatePriceRaiseIsNotMoreThanTwentyPercent(foundDish.Cost, (double)updateDishDto.Cost);
            ValidateNoConflictInDatabase.ValidateUpdateUniqueDishName(id, updateDishDto.Name, _dishRepository.GetAllDishes());

            foundDish.Name = updateDishDto.Name;
            foundDish.MadeBy = updateDishDto.MadeBy;
            foundDish.Cost = (double) updateDishDto.Cost;

            _dishRepository.UpdateDish(foundDish);

            ReadDishDto dishDto = _mapper.Map<ReadDishDto>(foundDish);
            _logger.LogInformation("Updated dish with id: " + id + ".");
            return Ok(dishDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDishById(int id)
        {
            _dishRepository.DeleteDishById(id);
            _logger.LogInformation("Deleted dish with id: " + id + ".");
            return Ok("Deleted dish with id: " + id);
        }
    }
}