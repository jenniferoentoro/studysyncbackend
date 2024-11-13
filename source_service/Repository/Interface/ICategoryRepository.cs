using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;

namespace source_service.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryById(string id);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> AddCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task<Category> DeleteCategory(string id);

        bool GetCategoryByName(string name);


    }
}