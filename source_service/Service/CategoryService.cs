using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Service.Interface;

namespace source_service.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> AddCategory(Category category)
        {
            return await _categoryRepository.AddCategory(category);
        }

        public async Task<Category> DeleteCategory(string id)
        {
            return await _categoryRepository.DeleteCategory(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategories();
        }

        public async Task<Category> GetCategoryById(string id)
        {
            return await _categoryRepository.GetCategoryById(id);
        }

        public bool GetCategoryByName(string name)
        {
            return _categoryRepository.GetCategoryByName(name);
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            return await _categoryRepository.UpdateCategory(category);
        }
    }
}