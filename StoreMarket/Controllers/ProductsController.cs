using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using StoreMarket.Abstraction;
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
        private readonly IProductServices _productServices;
        

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
            
        }

        [HttpGet]
        [Route("products/{id}")]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            var product = _productServices.GetProductById(id);
            return Ok(product);
        }


        [HttpGet]
        [Route("products")]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
           var products = _productServices.GetProducts();
            return Ok(products);
        }

        [HttpPost]
        [Route("products")]
        public ActionResult<int> AddProducts(ProductCreateRequest request)
        {
            try
            {
                int id = _productServices.AddProduct(request);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpDelete]
        [Route("products")]
        public ActionResult<int> DeleteProduct(ProductDeleteRequest request)
        {
            try
            {
                int idDeletingProduct = _productServices.DeleteProduct(request.Id);
                return Ok(idDeletingProduct);
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




        // Изменение цены через id и Price
        /*[HttpGet]
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
        }*/


        /*[HttpPut]
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
       */

        [HttpGet(template: "GetProductsCSV")]
        public FileContentResult GetProductsCSV()
        {

            var content = "";
            var products = _productServices.GetProducts();
            content = _productServices.GetCsv(products);
            return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
         }

        //private StoreContext storeContext;

        /*public ProductsController(StoreContext context)
        {
            storeContext = context;

        }*/
    }
}
