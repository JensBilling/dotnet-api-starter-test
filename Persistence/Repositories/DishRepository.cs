using dotnet_api_test.Exceptions.ExceptionResponses;
using dotnet_api_test.Persistence.Repositories.Interfaces;

namespace dotnet_api_test.Persistence.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _context;

        public DishRepository(AppDbContext context)
        {
            _context = context;
        }

        void IDishRepository.SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Dish> GetAllDishes()
        {
            if (!_context.Dishes.Any())
            {
                throw new NotFoundRequestExceptionResponse("No dishes found in database", 404);
            }

            return _context.Dishes;
        }

        public dynamic? GetAverageDishPrice()
        {
            return _context.Dishes.Average(dish => dish.Cost);
        }

        public Dish GetDishById(int id)
        {
            Dish dish = _context.Dishes.Find(id)!;
            if (dish == null)
            {
                throw new NotFoundRequestExceptionResponse("No dish found with id: " + id, 404);
            }

            return dish;
        }

        public void DeleteDishById(int id)
        {
            if (_context.Dishes.Find(id) == null)
            {
                throw new NotFoundRequestExceptionResponse("No dish found with id: " + id, 404);
            }
            
            _context.Dishes.Remove(_context.Dishes.Find(id)!);
            _context.SaveChanges();
        }

        public Dish CreateDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            _context.SaveChanges();
            return dish;
        }

        public Dish UpdateDish(Dish dish)
        {
            _context.Dishes.Update(dish);
            _context.SaveChanges();
            return dish;
        }
    }
}