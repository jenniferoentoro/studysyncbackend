using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using source_service.Dtos.Category;
using source_service.Dtos.Error;
using source_service.Model;
using source_service.Service.Interface;
using System.Diagnostics;
namespace source_service.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] CreateCategoryDto category)
        {
            bool ifExists = _categoryService.GetCategoryByName(category.Name);
            if (ifExists)
            {
                return BadRequest(new CustomErrorResponse
                {
                    Type = "Duplicate Category Name",
                    Title = "One or more errors occurred.",
                    Status = 400,
                    Message = "Category name already exists",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var categoryModel = _mapper.Map<Category>(category);
            var newCategory = await _categoryService.AddCategory(categoryModel);
            return Ok(newCategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(string id, [FromBody] CreateCategoryDto category)
        {
            var categoryModel = await _categoryService.GetCategoryById(id);
            if (categoryModel == null)
            {
                return NotFound();
            }

            bool ifExists = _categoryService.GetCategoryByName(category.Name);
            if (ifExists)
            {
                return BadRequest(new CustomErrorResponse
                {
                    Type = "Duplicate Category Name",
                    Title = "One or more errors occurred.",
                    Status = 400,
                    Message = "Category name already exists",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            categoryModel.Name = category.Name;
            var updatedCategory = await _categoryService.UpdateCategory(categoryModel);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(string id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            var deletedCategory = await _categoryService.DeleteCategory(id);
            return Ok(deletedCategory);
        }


    }
}