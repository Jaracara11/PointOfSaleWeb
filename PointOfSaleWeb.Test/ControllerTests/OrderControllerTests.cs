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
    public class OrderControllerTests
    {
        private readonly IOrderRepository _ordersRepo;
        private readonly OrderController _controller;
        private readonly List<Discount> _discounts;
        private readonly Dictionary<string, List<decimal>> _expectedDiscountsByUser;
        private readonly List<RecentOrderDTO> _recentOrders;
        private readonly List<RecentOrderDTO> _expectedOrders;

        public OrderControllerTests()
        {
            _ordersRepo = A.Fake<IOrderRepository>();
            _controller = new OrderController(_ordersRepo);

            _discounts =
            [
                new() { UserRoleID = 1, DiscountAmount = 10.0m },
                new() { UserRoleID = 2, DiscountAmount = 15.0m },
                new() { UserRoleID = 3, DiscountAmount = 20.0m }
            ];

            _expectedDiscountsByUser = new Dictionary<string, List<decimal>>
            {
                { "adminUser", new List<decimal> { 0.20m, 0.15m, 0.10m, 0.05m } },
                { "managerUser", new List<decimal> { 0.20m, 0.15m, 0.10m, 0.05m } },
                { "cashierUser", new List<decimal> { 0.10m, 0.05m } }
            };

            _recentOrders =
            [
                new()
                {
                    User = "user1",
                    OrderID = Guid.NewGuid(),
                    OrderTotal = 100.00m,
                    OrderDate = DateTime.UtcNow.AddDays(-1)
                },
                new()
                {
                    User = "user2",
                    OrderID = Guid.NewGuid(),
                    OrderTotal = 150.00m,
                    OrderDate = DateTime.UtcNow.AddDays(-2)
                },
                new()
                {
                    User = "user3",
                    OrderID = Guid.NewGuid(),
                    OrderTotal = 200.00m,
                    OrderDate = DateTime.UtcNow.AddDays(-3)
                }
            ];

            _expectedOrders =
        [
            new()
            {
                User = "John Doe",
                OrderID = Guid.NewGuid(),
                OrderTotal = 100.50m,
                OrderDate = new DateTime(2023, 06, 15)
            },
            new()
            {
                User = "Jane Smith",
                OrderID = Guid.NewGuid(),
                OrderTotal = 200.75m,
                OrderDate = new DateTime(2023, 09, 20)
            },
            new()
            {
                User = "Alice Johnson",
                OrderID = Guid.NewGuid(),
                OrderTotal = 150.00m,
                OrderDate = new DateTime(2023, 10, 05)
            }
        ];
        }

        [Fact]
        public async Task GetAllDiscounts_Returns_OK()
        {
            // Arrange
            A.CallTo(() => _ordersRepo.GetAllDiscounts()).Returns(Task.FromResult((IEnumerable<Discount>)_discounts));

            // Act
            var result = await _controller.GetAllDiscounts();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedDiscounts = okResult.Value.Should().BeOfType<List<Discount>>().Subject;

            returnedDiscounts.Count.Should().Be(_discounts.Count);
            returnedDiscounts.Should().BeEquivalentTo(_discounts);
        }

        [Fact]
        public async Task GetAvailableDiscountsByUsername_Returns_OK()
        {
            foreach (var user in _expectedDiscountsByUser)
            {
                var username = user.Key;
                var expectedDiscounts = user.Value;

                // Arrange
                A.CallTo(() => _ordersRepo.GetDiscountsByUsername(username))
                    .Returns(Task.FromResult((IEnumerable<decimal>)expectedDiscounts));

                // Act
                var result = await _controller.GetAvailableDiscountsByUsername(username);

                // Assert
                result.Should().NotBeNull();
                var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
                var returnedDiscounts = okResult.Value.Should().BeOfType<List<decimal>>().Subject;

                returnedDiscounts.Count.Should().Be(expectedDiscounts.Count);
                returnedDiscounts.Should().BeEquivalentTo(expectedDiscounts);
            }
        }

        [Fact]
        public async Task GetRecentOrders_Returns_OK()
        {
            // Arrange
            A.CallTo(() => _ordersRepo.GetRecentOrders())
                .Returns(Task.FromResult((IEnumerable<RecentOrderDTO>)_recentOrders));

            // Act
            var result = await _controller.GetRecentOrders();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedOrders = okResult.Value.Should().BeOfType<List<RecentOrderDTO>>().Subject;

            returnedOrders.Count.Should().Be(_recentOrders.Count);
            returnedOrders.Should().BeEquivalentTo(_recentOrders);
        }

        [Fact]
        public async Task GetOrderByID_Returns_OK()
        {
            // Arrange
            var orderId = "some-valid-id";
            var expectedOrder = new OrderDTO
            {
                OrderID = Guid.NewGuid(),
                User = "user1",
                Products = "ProductA, ProductB",
                OrderSubTotal = 100.00m,
                Discount = 10.00m,
                OrderTotal = 90.00m,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                OrderCancelled = false
            };

            A.CallTo(() => _ordersRepo.GetOrderByID(orderId))
                .Returns(Task.FromResult((OrderDTO?)expectedOrder));

            // Act
            var result = await _controller.GetOrderByID(orderId);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedOrder = okResult.Value.Should().BeOfType<OrderDTO>().Subject;

            returnedOrder.Should().BeEquivalentTo(expectedOrder);
        }

        [Fact]
        public async Task GetTotalSalesOfTheDay_Returns_OK()
        {
            // Arrange
            var expectedSalesTotal = 500.00m;

            A.CallTo(() => _ordersRepo.GetTotalSalesOfTheDay())
                .Returns(Task.FromResult(expectedSalesTotal));

            // Act
            var result = await _controller.GetTotalSalesOfTheDay();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedTotal = okResult.Value.Should().BeOfType<decimal>().Subject;

            returnedTotal.Should().Be(expectedSalesTotal);
        }

        [Fact]
        public async Task GetSalesByDate_ValidDateRange_Returns_OK()
        {
            // Arrange
            var initialDate = new DateTime(2023, 01, 01);
            var finalDate = new DateTime(2023, 12, 31);
            var expectedSalesTotal = 1500.00m;

            A.CallTo(() => _ordersRepo.GetSalesByDate(initialDate, finalDate))
                .Returns(Task.FromResult(expectedSalesTotal));

            // Act
            var result = await _controller.GetSalesByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedTotal = okResult.Value.Should().BeOfType<decimal>().Subject;

            returnedTotal.Should().Be(expectedSalesTotal);
        }

        [Fact]
        public async Task GetSalesByDate_InvalidDateRange_Returns_BadRequest()
        {
            // Arrange
            var initialDate = new DateTime(2023, 12, 31);
            var finalDate = new DateTime(2023, 01, 01);
            var expectedMessage = "Start date cannot be greater than end date.";

            A.CallTo(() => _ordersRepo.GetSalesByDate(A<DateTime>.Ignored, A<DateTime>.Ignored))
                .Returns(Task.FromResult(0m));

            // Act
            var result = await _controller.GetSalesByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorResponse = badRequestResult.Value.Should().BeEquivalentTo(new { Message = expectedMessage });
        }

        [Fact]
        public async Task GetSalesByDate_Null_Dates_Returns_BadRequest()
        {
            // Arrange
            DateTime? initialDate = null;
            DateTime? finalDate = null;
            var expectedMessage = "Both start date and end date are required.";

            A.CallTo(() => _ordersRepo.GetSalesByDate(A<DateTime>.Ignored, A<DateTime>.Ignored))
                .Returns(Task.FromResult(0m));

            // Act
            var result = await _controller.GetSalesByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorResponse = badRequestResult.Value.Should().BeEquivalentTo(new { Message = expectedMessage });
        }

        [Fact]
        public async Task GetOrdersByDate_ValidDateRange_Returns_OK()
        {
            // Arrange
            var initialDate = new DateTime(2023, 01, 01);
            var finalDate = new DateTime(2023, 12, 31);

            A.CallTo(() => _ordersRepo.GetOrdersByDate(initialDate, finalDate))
                .Returns(Task.FromResult((IEnumerable<RecentOrderDTO>)_expectedOrders));

            // Act
            var result = await _controller.GetOrdersByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedOrders = okResult.Value.Should().BeOfType<List<RecentOrderDTO>>().Subject;
            returnedOrders.Should().BeEquivalentTo(_expectedOrders);
        }

        [Fact]
        public async Task GetOrdersByDate_InvalidDateRange_Returns_BadRequest()
        {
            // Arrange
            var initialDate = new DateTime(2023, 12, 31);
            var finalDate = new DateTime(2023, 01, 01);
            var expectedMessage = "Start date cannot be greater than end date.";

            A.CallTo(() => _ordersRepo.GetOrdersByDate(A<DateTime>.Ignored, A<DateTime>.Ignored))
                .Returns(Task.FromResult(Enumerable.Empty<RecentOrderDTO>()));

            // Act
            var result = await _controller.GetOrdersByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorResponse = badRequestResult.Value.Should().BeEquivalentTo(new { Message = expectedMessage });
        }

        [Fact]
        public async Task GetOrdersByDate_Null_Dates_Returns_BadRequest()
        {
            // Arrange
            DateTime? initialDate = null;
            DateTime? finalDate = null;
            var expectedMessage = "Both start date and end date are required.";

            A.CallTo(() => _ordersRepo.GetOrdersByDate(A<DateTime>.Ignored, A<DateTime>.Ignored))
                .Returns(Task.FromResult(Enumerable.Empty<RecentOrderDTO>()));

            // Act
            var result = await _controller.GetOrdersByDate(initialDate, finalDate);

            // Assert
            result.Should().NotBeNull();
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var errorResponse = badRequestResult.Value.Should().BeEquivalentTo(new { Message = expectedMessage });
        }

        [Fact]
        public async Task CheckoutOrder_Success_Returns_Created()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                User = "test_user",
                Products =
                [
                  new() { ProductID = "prod_001", ProductQuantity = 1 },
                  new() { ProductID = "prod_002", ProductQuantity = 2 },
                  new() { ProductID = "prod_003", ProductQuantity = 3 }
                ],
                Discount = 5.00m
            };

            var expectedOrder = new OrderDTO
            {
                OrderID = Guid.NewGuid(),
                User = orderRequest.User,
                OrderTotal = 100.00m,
                OrderDate = DateTime.UtcNow
            };

            var response = new TestDbResponse<OrderDTO>(true, expectedOrder);

            A.CallTo(() => _ordersRepo.NewOrderTransaction(orderRequest))
                .Returns(Task.FromResult((DbResponse<OrderDTO>)response));

            // Act
            var result = await _controller.CheckoutOrder(orderRequest);

            // Assert
            result.Should().NotBeNull();
            var createdResult = result.Result.Should().BeOfType<CreatedResult>().Subject;
            createdResult.Location.Should().Be("Order");
            createdResult.Value.Should().BeEquivalentTo(expectedOrder);
        }

        [Fact]
        public async Task CancelOrder_Success_Returns_NoContent()
        {
            // Arrange
            var orderId = "999999";
            var response = new DbResponse<string> { Success = true };

            A.CallTo(() => _ordersRepo.CancelOrder(orderId))
                .Returns(Task.FromResult(response));

            // Act
            var result = await _controller.CancelOrder(orderId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}

