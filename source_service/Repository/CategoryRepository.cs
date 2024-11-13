using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Data;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace source_service.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(IOptions<DatabaseConfiguration> categoryConfiguration)
        {
            var mongoClient = new MongoClient(categoryConfiguration.Value.ConnectionString);
            var database = mongoClient.GetDatabase(categoryConfiguration.Value.DatabaseNameSource);

            _categories = database.GetCollection<Category>(categoryConfiguration.Value.CategoriesCollectionName);
        }
        public async Task<Category> AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await _categories.InsertOneAsync(category);
            return category;

        }

        public async Task<Category> DeleteCategory(string id)
        {
            var category = await GetCategoryById(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            await _categories.DeleteOneAsync(c => c.Id == id);
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await _categories.Find(_ => true).ToListAsync();
            return categories;
        }

        public async Task<Category?> GetCategoryById(string id)
        {
            var category = await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();
            return category;
        }

        public bool GetCategoryByName(string name)
        {
            var category = _categories.Find(c => c.Name == name).FirstOrDefault();
            return category != null;

        }

        public async Task<Category> UpdateCategory(Category category)
        {
            var existingCategory = await GetCategoryById(category.Id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }

            var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);
            var update = Builders<Category>.Update.Set(c => c.Name, category.Name);
            await _categories.UpdateOneAsync(filter, update);

            return category;
        }
    }
}