using StoreMarket.Models;
using System.ComponentModel.DataAnnotations;

namespace StoreMarket.Contracts.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public int? CategoryId { get; set; }
        

        /*public ProductResponse(Product products)
        {
            Id = products.Id;
            Name = products.Name;
            Description = products.Description;
            Price = products.Price;
            CategoryId = products.CategoryId;
        }*/


    }

}
