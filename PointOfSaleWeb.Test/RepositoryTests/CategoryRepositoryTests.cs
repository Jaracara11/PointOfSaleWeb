using Dapper;
using FakeItEasy;
using FluentAssertions;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository;
using PointOfSaleWeb.Repository.Repositories;
using System.Data;

namespace PointOfSaleWeb.Tests.RepositoryTests
{
    public class CategoryRepositoryTests
    {
        private readonly IDbConnection _fakeDbConnection;
        private readonly DbContext _fakeDbContext;
        private readonly CategoryRepository _categoryRepository;
        private readonly List<Category> _categories;

        public CategoryRepositoryTests(List<Category> categories)
        {
            _fakeDbConnection = A.Fake<IDbConnection>();
            _fakeDbContext = A.Fake<DbContext>();
            A.CallTo(() => _fakeDbContext.CreateConnection()).Returns(_fakeDbConnection);
            _categoryRepository = new CategoryRepository(_fakeDbContext);

            _categories =
            [
                new Category { CategoryID = 1, CategoryName = "Beverages" },
                new Category { CategoryID = 2, CategoryName = "Snacks" },
                new Category { CategoryID = 3, CategoryName = "Desserts" }
            ];

            _categories = categories;
        }

        [Fact]
        public async Task GetAllCategories_Returns_Category_List()
        {
            // Arrange
            A.CallTo(() => _fakeDbConnection.QueryAsync<Category>(
                "GetAllCategories", null, null, null, CommandType.StoredProcedure))
                .Returns(_categories);

            // Act
            var result = await _categoryRepository.GetAllCategories();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_categories);
        }

        [Fact]
        public async Task GetCategoryByID_Returns_Category_Object()
        {
            // Arrange
            var categoryId = 1;
            var expectedCategory = _categories.Find(c => c.CategoryID == categoryId);

            A.CallTo(() => _fakeDbConnection.QuerySingleOrDefaultAsync<Category>(
                "GetCategoryById", A<object>.That.Matches(p => ((dynamic)p).CategoryID == categoryId),
                null, null, CommandType.StoredProcedure))
                .Returns(expectedCategory);

            // Act
            var result = await _categoryRepository.GetCategoryByID(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedCategory);
        }

        [Fact]
        public async Task AddNewCategory_Returns_Success_When_Added()
        {
            // Arrange
            var categoryName = "Chocolates";
            var newCategory = new Category { CategoryID = 4, CategoryName = categoryName };

            A.CallTo(() => _fakeDbConnection.QuerySingleOrDefaultAsync<Category>(
                "AddNewCategory", A<object>.That.Matches(p => ((dynamic)p).CategoryName == categoryName),
                null, null, CommandType.StoredProcedure))
                .Returns(newCategory);

            // Act
            var result = await _categoryRepository.AddNewCategory(categoryName);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(newCategory);
        }

        [Fact]
        public async Task UpdateCategory_Returns_Success_When_Updated()
        {
            // Arrange
            var updatedCategory = new Category { CategoryID = 1, CategoryName = "Updated Category" };

            A.CallTo(() => _fakeDbConnection.QuerySingleOrDefaultAsync<Category>(
                "UpdateCategory", A<object>.Ignored, null, null, System.Data.CommandType.StoredProcedure))
                .Returns(updatedCategory);

            // Act
            var result = await _categoryRepository.UpdateCategory(updatedCategory);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(updatedCategory);
        }

        [Fact]
        public async Task DeleteCategory_Returns_Success_When_Deleted()
        {
            // Arrange
            var categoryId = 1;

            A.CallTo(() => _fakeDbConnection.ExecuteAsync(
                "DeleteCategory", A<object>.That.Matches(p => ((dynamic)p).CategoryID == categoryId),
                null, null, System.Data.CommandType.StoredProcedure))
                .Returns(1);  // Returns the number of rows affected

            // Act
            var result = await _categoryRepository.DeleteCategory(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }
    }
}
