﻿using Dapper.Contrib.Extensions;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class CategoryRepository(DbContext context) : ICategoryRepository
    {
        private readonly DbContext _context = context;

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.GetAllAsync<Category>();
        }

        public async Task<Category?> GetCategoryByID(int id)
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.GetAsync<Category>(id);
        }

        public async Task<bool> AddNewCategory(string categoryName)
        {
            using IDbConnection db = _context.CreateConnection();

            var category = new Category
            {
                CategoryName = categoryName
            };

            var categoryID = await db.InsertAsync(category);

            return categoryID > 0;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            using IDbConnection db = _context.CreateConnection();

            var result = await db.UpdateAsync(category);

            if (result)
                return category;

            throw new Exception("Failed to update category.");
        }

        public async Task<bool> DeleteCategory(int id)
        {
            using IDbConnection db = _context.CreateConnection();

            var category = new Category { CategoryID = id };

            return await db.DeleteAsync(category);
        }
    }
}