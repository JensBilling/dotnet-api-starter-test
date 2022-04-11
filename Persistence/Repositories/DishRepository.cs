using System.Collections.Generic;
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
            return _context.Dishes;
        }

        public dynamic? GetAverageDishPrice()
        {
            return _context.Dishes.Average(dish => dish.Cost);
        }

        public Dish GetDishById(int Id)
        {
            Dish dish = _context.Dishes.Find(Id);
            return dish;
        }

        public void DeleteDishById(int Id)
        {
            throw new System.NotImplementedException();
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