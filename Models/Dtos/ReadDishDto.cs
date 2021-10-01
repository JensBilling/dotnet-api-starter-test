using System;

namespace dotnet_api_test.Models.Dtos
{
    public class ReadDishDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public string MadeBy { get; set; }
        
        public double Cost { get; set; }
    }
}