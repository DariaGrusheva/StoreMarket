using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;

namespace StoreMarket.Abstraction
{
    public interface IProductServices

    {
        public int AddProduct(ProductCreateRequest product);

        public IEnumerable<ProductResponse> GetProducts();
        public ProductResponse GetProductById(int id);
        public int DeleteProduct(ProductDeleteRequest product);
        public int DeleteProduct(int id);
        public string GetCsv(IEnumerable<ProductResponse> products);
        }
}
