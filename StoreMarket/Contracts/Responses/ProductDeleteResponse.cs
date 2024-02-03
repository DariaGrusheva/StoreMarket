using StoreMarket.Models;

namespace StoreMarket.Contracts.Responses
{
    public class ProductDeleteResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public ProductDeleteResponse(Product products)
        {
            Id = products.Id;
            Name = products.Name;
        }


    }
}