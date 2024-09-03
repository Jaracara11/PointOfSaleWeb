using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.App.Controllers;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using PointOfSaleWeb.Tests.Utilities;

namespace PointOfSaleWeb.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        private readonly IProductRepository _prodRepo;
        private readonly ProductController _controller;
        private readonly List<Product> _products;
        private readonly List<BestSellerProductDTO> _bestSellers;
        private readonly List<ProductSoldByDateDTO> _productsSoldByDate;

        public ProductControllerTests()
        {
            _prodRepo = A.Fake<IProductRepository>();
            _controller = new ProductController(_prodRepo);

            _products =
            [
                new() {
                    ProductID = "451236",
                    ProductName = "The Great Gatsby",
                    ProductDescription = "Classic novel by F. Scott Fitzgerald",
                    ProductStock = 72,
                    ProductQuantity = 0,
                    ProductCost = 5.00m,
                    ProductPrice = 10.99m,
                    ProductCategory = new Category { CategoryID = 4, CategoryName = "Books" }
                },
                new()
                {
                    ProductID = "7854123",
                    ProductName = "KitchenAid Stand Mixer",
                    ProductDescription = "High-performance kitchen appliance",
                    ProductStock = 5,
                    ProductQuantity = 0,
                    ProductCost = 250.00m,
                    ProductPrice = 299.99m,
                    ProductCategory = new Category { CategoryID = 3, CategoryName = "Home and Kitchen" }
                },
                new()
                {
                    ProductID = "123854",
                    ProductName = "Sony PlayStation 5",
                    ProductDescription = "Next-gen gaming console",
                    ProductStock = 32,
                    ProductQuantity = 0,
                    ProductCost = 450.00m,
                    ProductPrice = 499.99m,
                    ProductCategory = new Category { CategoryID = 1, CategoryName = "Electronics" }
                }
            ];

            _bestSellers =
            [
                new() {
                    ProductName = "The Great Gatsby",
                    ProductDescription = "Classic novel by F. Scott Fitzgerald",
                    TotalQuantitySold = 150
                },
                new() {
                    ProductName = "KitchenAid Stand Mixer",
                    ProductDescription = "High-performance kitchen appliance",
                    TotalQuantitySold = 200
                },
                new() {
                    ProductName = "Sony PlayStation 5",
                    ProductDescription = "Next-gen gaming console",
                    TotalQuantitySold = 75
                }
            ];

            _productsSoldByDate =
            [
                new()
                {
                    ProductID = "451236",
                    ProductName = "The Great Gatsby",
                    ProductDescription = "Classic novel by F. Scott Fitzgerald",
                    TotalUnitsSold = 120,
                    TotalSold = 1318.80m
                },
                new()
                {
                    ProductID = "7854123",
                    ProductName = "KitchenAid Stand Mixer",
                    ProductDescription = "High-performance kitchen appliance",
                    TotalUnitsSold = 35,
                    TotalSold = 10499.65m
                },
                new()
                {
                    ProductID = "123854",
                    ProductName = "Sony PlayStation 5",
                    ProductDescription = "Next-gen gaming console",
                    TotalUnitsSold = 50,
                    TotalSold = 24999.50m
                }
            ];
        }

        [Fact]
        public async Task GetAllProducts_Returns_OK()
        {
            // Arrange
            A.CallTo(() => _prodRepo.GetAllProducts()).Returns(Task.FromResult((IEnumerable<Product>)_products));

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProducts = okResult.Value.Should().BeOfType<List<Product>>().Subject;

            returnedProducts.Count.Should().Be(_products.Count);
            returnedProducts.Should().BeEquivalentTo(_products);
        }

        [Fact]
        public async Task GetProductByID_Returns_OK()
        {
            // Arrange
            var productId = "451236";
            var expectedProduct = _products.Find(p => p.ProductID == productId);

            A.CallTo(() => _prodRepo.GetProductByID(productId)).Returns(Task.FromResult(expectedProduct));

            // Act
            var result = await _controller.GetProductByID(productId);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actualProduct = okResult.Value.Should().BeOfType<Product>().Subject;
            actualProduct.Should().BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public async Task GetBestSellerProducts_Returns_OK()
        {
            // Arrange
            A.CallTo(() => _prodRepo.GetBestSellerProducts()).Returns(Task.FromResult((IEnumerable<BestSellerProductDTO>)_bestSellers));

            // Act
            var result = await _controller.GetBestSellerProducts();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedBestSellers = okResult.Value.Should().BeOfType<List<BestSellerProductDTO>>().Subject;

            returnedBestSellers.Count.Should().Be(_bestSellers.Count);
            returnedBestSellers.Should().BeEquivalentTo(_bestSellers);
        }

        [Fact]
        public async Task GetProductsSoldByDate_ValidDates_Returns_OK()
        {
            // Arrange
            var initialDate = new DateTime(2023, 01, 01);
            var finalDate = new DateTime(2023, 12, 31);
            A.CallTo(() => _prodRepo.GetProductsSoldByDate(initialDate, finalDate))
                .Returns(Task.FromResult((IEnumerable<ProductSoldByDateDTO>)_productsSoldByDate));

            // Act
            var result = await _controller.GetProductsSoldByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProducts = okResult.Value.Should().BeOfType<List<ProductSoldByDateDTO>>().Subject;

            returnedProducts.Count.Should().Be(_productsSoldByDate.Count);
            returnedProducts.Should().BeEquivalentTo(_productsSoldByDate);
        }

        [Fact]
        public async Task GetProductsSoldByDate_InvalidDateRange_Returns_BadRequest()
        {
            // Arrange
            var initialDate = new DateTime(2023, 12, 31);
            var finalDate = new DateTime(2023, 01, 01);
            var expectedMessage = "Start date cannot be greater than end date.";
            A.CallTo(() => _prodRepo.GetProductsSoldByDate(A<DateTime>.Ignored, A<DateTime>.Ignored))
                .Returns(Task.FromResult((IEnumerable<ProductSoldByDateDTO>)[]));

            // Act
            var result = await _controller.GetProductsSoldByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorResponse = badRequestResult.Value.Should().BeEquivalentTo(new { Message = expectedMessage });
        }

        [Fact]
        public async Task GetProductsSoldByDate_Null_Dates_Returns_BadRequest()
        {
            // Arrange
            DateTime? initialDate = null;
            DateTime? finalDate = null;
            var expectedMessage = "Both start date and end date are required.";
            A.CallTo(() => _prodRepo.GetProductsSoldByDate(A<DateTime>.Ignored, A<DateTime>.Ignored))
                .Returns(Task.FromResult((IEnumerable<ProductSoldByDateDTO>)[]));

            // Act
            var result = await _controller.GetProductsSoldByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorResponse = badRequestResult.Value.Should().BeEquivalentTo(new { Message = expectedMessage });
        }

        [Fact]
        public async Task AddNewProduct_Success_Returns_Created()
        {
            // Arrange
            var newProduct = new Product
            {
                ProductID = "999999",
                ProductName = "New Product",
                ProductDescription = "Description of new product",
                ProductStock = 10,
                ProductQuantity = 5,
                ProductCost = 10.00m,
                ProductPrice = 20.00m,
                ProductCategory = new Category { CategoryID = 2, CategoryName = "Electronics" }
            };

            var response = new TestDbResponse<Product>(true, newProduct);

            A.CallTo(() => _prodRepo.AddNewProduct(newProduct)).Returns(Task.FromResult((DbResponse<Product>)response));

            // Act
            var result = await _controller.AddNewProduct(newProduct);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedResult>().Subject;
            createdResult.Location.Should().Be("Product");
            createdResult.Value.Should().BeEquivalentTo(newProduct);
        }

        [Fact]
        public async Task UpdateProduct_Success_Returns_Ok()
        {
            // Arrange
            var updatedProduct = new Product
            {
                ProductID = "999999",
                ProductName = "Updated Product",
                ProductDescription = "Updated description",
                ProductStock = 20,
                ProductQuantity = 10,
                ProductCost = 15.00m,
                ProductPrice = 25.00m,
                ProductCategory = new Category { CategoryID = 2, CategoryName = "Electronics" }
            };

            var response = new TestDbResponse<Product>(true, updatedProduct);

            A.CallTo(() => _prodRepo.UpdateProduct(updatedProduct))
                .Returns(Task.FromResult((DbResponse<Product>)response));

            // Act
            var result = await _controller.UpdateProduct(updatedProduct);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task DeleteProduct_Success_Returns_NoContent()
        {
            // Arrange
            var productId = "999999";
            var response = new DbResponse<Product> { Success = true };

            A.CallTo(() => _prodRepo.DeleteProduct(productId))
                .Returns(Task.FromResult(response));

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}