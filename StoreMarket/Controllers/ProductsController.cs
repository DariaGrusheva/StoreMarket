using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Route("products/{id}")]

        public ActionResult<ProductResponse> GetProduct(int id)
        {
            var result = storeContext.Products.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new ProductResponse(result));
            }

        }


        [HttpGet]
        [Route("products")]

        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            IEnumerable<Product> result = storeContext.Products;

           // IEnumerable<ProductResponse> products = result.Select(result => new ProductResponse(result));

            return Ok(result.Select(result => new ProductResponse(result)));
        }

        [HttpPost]
        [Route("products")]

        public ActionResult<ProductResponse> AddProducts(ProductCreateRequest request)
        {
            Product product = request.ProductGetEntity();
            try
            {
                var result = storeContext.Products.Add(product).Entity;

                storeContext.SaveChanges();
                return Ok(new ProductResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // Удаление по Id
/*        [HttpPost]
        [Route("products")]
        public ActionResult<ProductResponse> DeleteProducts(int id)
        {
                Product? products = storeContext.Products.FirstOrDefault(p => p.Id == id); 
                if (products != null)
                {
                    storeContext.Products.Remove(products);
                    storeContext.SaveChanges();
                    return Ok(new ProductDeleteResponse(products));
                }
            return NotFound();
        }*/



        [HttpDelete]
        [Route("products")]
        public ActionResult<ProductResponse> DeleteProducts(ProductDeleteRequest request)
        {

            Product? product = storeContext.Products.FirstOrDefault(p => p.Id == request.Id);
            if (product != null)
            {
                storeContext.Products.Remove(product); 
                storeContext.SaveChanges();
                return Ok(new ProductDeleteResponse(product));
            }
            return NotFound();
        }

        // Изменение цены через id и Price
        [HttpGet]
        [Route("changeprice")]
        public ActionResult<ProductResponse> PriceProducts(int id, int price)
        {
            Product? product = storeContext.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Price = price;
                storeContext.SaveChanges();
                return Ok(new ProductResponse(product)); ;
            }
            return NotFound();
        }


        [HttpPut]
        [Route("products")]

        public ActionResult<ProductResponse> PriceProducts(ProductUpdateRequest request)
        {
            Product product = request.ProductGetEntity();
            try
            {
                Product result = storeContext.Update(product).Entity;
                storeContext.SaveChanges();
                return Ok(new ProductResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       
        private StoreContext storeContext;

        public ProductsController(StoreContext context)
        {
            storeContext = context;

        }
    }
}
