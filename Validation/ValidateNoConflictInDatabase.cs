using dotnet_api_test.Exceptions.ExceptionResponses;

namespace dotnet_api_test.Validation;

public static class ValidateNoConflictInDatabase
{
    public static void ValidateCreateUniqueDishName(string name, IEnumerable<Dish> dishes)
    {
        foreach (Dish dish in dishes)
        {
            if (dish.Name.Equals(name))
            {
                throw new BadRequestExceptionResponse("You are not allowed to call two dishes the same name", 400);
            }
        }
    }

    public static void ValidateUpdateUniqueDishName(int id, string name, IEnumerable<Dish> dishes)
    {
        foreach (Dish dish in dishes)
        {
            if (dish.Name.Equals(name) && id != dish.Id)
            {
                throw new BadRequestExceptionResponse("You are not allowed to call two dishes the same name", 400);
            }
        }
    }

    public static void ValidatePriceRaiseIsNotMoreThanTwentyPercent(double oldPrice, double newPrice)
    {
        if (newPrice > oldPrice * 1.2)
        {
            throw new BadRequestExceptionResponse("You are not allowed to raise the price more than 20%", 400);
        }
    }
}