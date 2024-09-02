using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.App.Controllers;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;
using PointOfSaleWeb.Tests.Utilities;

namespace PointOfSaleWeb.Tests.ControllerTests
{
    public class CategoryControllerTests
    {
        private readonly ICategoryRepository _catRepo;
        private readonly CategoryController _controller;
        private readonly List<Category> _categories;

        public CategoryControllerTests()
        {
            _catRepo = A.Fake<ICategoryRepository>();
            _controller = new CategoryController(_catRepo);

            _categories =
            [
                new() { CategoryID = 1, CategoryName = "Beverages" },
                new() { CategoryID = 2, CategoryName = "Snacks" },
                new() { CategoryID = 3, CategoryName = "Desserts" }
            ];
        }

        [Fact]
        public async Task CategoryController_GetAllCategories_Returns_OK()
        {
            // Arrange
            A.CallTo(() => _catRepo.GetAllCategories()).Returns(Task.FromResult((IEnumerable<Category>)_categories));

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedCategories = okResult.Value.Should().BeOfType<List<Category>>().Subject;

            returnedCategories.Count.Should().Be(_categories.Count);
            returnedCategories.Should().BeEquivalentTo(_categories);
        }

        [Fact]
        public async Task CategoryController_GetCategoryByID_Returns_OK()
        {
            // Arrange
            var categoryId = 1;
            var expectedCategory = _categories.Find(c => c.CategoryID == categoryId);

            A.CallTo(() => _catRepo.GetCategoryByID(categoryId)).Returns(Task.FromResult(expectedCategory));

            // Act
            var result = await _controller.GetCategoryByID(categoryId);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actualCategory = okResult.Value.Should().BeOfType<Category>().Subject;
            actualCategory.Should().BeEquivalentTo(expectedCategory);
        }

        [Fact]
        public async Task CategoryController_AddNewCategory_Success_Returns_Created()
        {
            // Arrange
            var category = new Category { CategoryName = "Chocolates" };
            var response = new TestDbResponse<Category>(true, category);

            A.CallTo(() => _catRepo.AddNewCategory(category.CategoryName))
             .Returns(Task.FromResult((DbResponse<Category>)response));

            // Act
            var result = await _controller.AddNewCategory(category);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedResult>().Subject;
            createdResult.Location.Should().Be("Category");
            createdResult.Value.Should().Be(category);
        }

        [Fact]
        public async Task CategoryController_UpdateCategory_Success_Returns_Ok()
        {
            // Arrange
            var category = new Category { CategoryID = 1, CategoryName = "Updated Category" };
            var response = new TestDbResponse<Category>(true, category);

            A.CallTo(() => _catRepo.UpdateCategory(category))
                .Returns(Task.FromResult((DbResponse<Category>)response));

            // Act
            var result = await _controller.UpdateCategory(category);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task CategoryController_DeleteCategory_Success_Return_NoContent()
        {
            // Arrange
            var categoryId = 1;
            var response = new DbResponse<Category> { Success = true };

            A.CallTo(() => _catRepo.DeleteCategory(categoryId))
                .Returns(Task.FromResult(response));

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
