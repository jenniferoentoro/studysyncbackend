using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;

namespace source_service.Service.Interface
{
    public interface ICategoryService
    {
        public Task<Category> GetCategoryById(string id);
        public Task<IEnumerable<Category>> GetAllCategories();
        public Task<Category> AddCategory(Category category);
        public Task<Category> UpdateCategory(Category category);
        public Task<Category> DeleteCategory(string id);

        public bool GetCategoryByName(string name);

    }
}