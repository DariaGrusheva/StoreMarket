﻿using Microsoft.AspNetCore.Mvc;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        [HttpGet]
        [Route("categories/{id}")]

        public ActionResult<CategoryResponse> GetCategory(int id)
        {
            var result = storeContext.Categories.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new CategoryResponse(result));
            }

        }


        [HttpGet]
        [Route("categories")]

        public ActionResult<IEnumerable<CategoryResponse>> GetCategories()
        {
            IEnumerable<Category> result = storeContext.Categories;

            return Ok(result.Select(result => new CategoryResponse(result)));
        }

        [HttpPost]
        [Route("categories")]

        public ActionResult<CategoryResponse> AddCategory(CategoryCreateRequest request)
        {
            Category category = request.CategoryGetEntity();
            try
            {
                var result = storeContext.Categories.Add(category).Entity;

                storeContext.SaveChanges();
                return Ok(new CategoryResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        [Route("categories")]
        public ActionResult<CategoryResponse> DeleteCategory(int id)
        {
            Category? categories = storeContext.Categories.FirstOrDefault(p => p.Id == id);
            if (categories != null)
            {
                storeContext.Categories.Remove(categories);
                storeContext.SaveChanges();
                return Ok(new CategoryDeleteResponse(categories));
            }
            return NotFound();
        }


        private StoreContext storeContext;

        public CategoriesController(StoreContext context)
        {
            storeContext = context;

        }
    }
}

