using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StoreMarket.Abstraction;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;
using System.Text;

namespace StoreMarket.Serveces
{
    public class ProductServices : IProductServices
    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;
        public readonly IMemoryCache _memoryCache;

        public ProductServices(StoreContext storeContext, IMapper mapper, IMemoryCache memoryCache)
        {
            _storeContext = storeContext;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public int AddProduct(ProductCreateRequest product)
        {
            var mappEntity = _mapper.Map<Product>(product);
            _storeContext.Products.Add(mappEntity);

            _storeContext.SaveChanges();
            _memoryCache.Remove("products");
            return mappEntity.Id;
        }

        public ProductResponse GetProductById(int id)
        {
            var product = _storeContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return null;
            }
            return _mapper.Map<ProductResponse>(product);
        }

        public IEnumerable<ProductResponse> GetProducts()
        {
            if (_memoryCache.TryGetValue("products", out IEnumerable<ProductResponse> prods))
            {
                return prods;
            }

            IEnumerable<ProductResponse> products = _storeContext.Products.Select(_mapper.Map<ProductResponse>).ToList();
            _memoryCache.Set("products", products, TimeSpan.FromMinutes(30));
            return products;
        }

        // удаление через Map
        public int DeleteProduct (ProductDeleteRequest product)
        {
            var mapEntity = _mapper.Map<Product>(product);
            _storeContext.Products.Remove(mapEntity);
            _storeContext.SaveChanges();
            return mapEntity.Id;
        }

        // удаление не через Map
        public int DeleteProduct(int id)
        {
            try
            {
                Product? products = _storeContext.Products.FirstOrDefault(p => p.Id == id);
                if (products != null)
                {
                    _storeContext.Products.Remove(products);
                    _storeContext.SaveChanges();
                    return id;
                }
                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


            

            // вспомогательный метод для возврата данных в формате CSV
            public string GetCsv(IEnumerable<ProductResponse> products)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {
                sb.AppendLine(product.Name + ": " + product.Description + "\n");
            }
            return sb.ToString();
        }
    }
}
