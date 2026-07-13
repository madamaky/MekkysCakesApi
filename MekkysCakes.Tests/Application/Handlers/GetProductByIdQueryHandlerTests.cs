using AutoMapper;
using FluentAssertions;
using MekkysCakes.Application.Features.Products.Queries.GetProductById;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;
using MekkysCakes.Shared.DTOs.ProductDTOs;
using Moq;

namespace MekkysCakes.Tests.Application.Handlers
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IGenericRepository<Product, int>> _mockProductRepo;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockProductRepo = new Mock<IGenericRepository<Product, int>>();

            _mockUnitOfWork
                .Setup(uow => uow.GetRepository<Product, int>())
                .Returns(_mockProductRepo.Object);

            _handler = new GetProductByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }


        #region Success Path Tests

        [Fact]
        public async Task Handle_Should_ReturnProduct_When_ProductExists()
        {
            // Arrange
            var productId = 1;
            var query = new GetProductByIdQuery(productId);

            var productFromDb = new Product
            {
                Id = productId,
                Price = 25.00m,
                PictureUrl = "https://example.com/cake.jpg",
                InStock = true,
                AverageRating = 4.5m,
                TotalReviews = 10,
                TypeId = 1,
                ThemeId = 1
            };
            var expectedDto = new ProductDTO
            {
                Id = productId,
                Price = 25.00m,
                PictureUrl = "https://example.com/cake.jpg",
                InStock = true,
                AverageRating = 4.5m,
                TotalReviews = 10,
                Name = new LocalizedString { En = "Chocolate Cake", Ar = "كيكة شوكولاتة" },
                ProductType = new LocalizedString { En = "Cake", Ar = "كيك" },
                ProductTheme = new LocalizedString { En = "Birthday", Ar = "عيد ميلاد" }
            };

            _mockProductRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<ISpecification<Product, int>>()))
                .ReturnsAsync(productFromDb);
            _mockMapper
                .Setup(m => m.Map<ProductDTO>(productFromDb))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue(because: "the product was found in the repository");
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(productId);
            result.Value.Price.Should().Be(25.00m);
            result.Value.PictureUrl.Should().Be("https://example.com/cake.jpg");
            result.Value.InStock.Should().BeTrue();
            result.Value.AverageRating.Should().Be(4.5m);
            result.Value.TotalReviews.Should().Be(10);
        }

        [Fact]
        public async Task Handle_Should_ReturnMappedDTO_When_ProductExists()
        {
            // Arrange
            var query = new GetProductByIdQuery(1);
            var product = new Product { Id = 1, Price = 15m, PictureUrl = "url" };
            var expectedDto = new ProductDTO
            {
                Id = 1,
                Price = 15m,
                Name = new LocalizedString { En = "Test", Ar = "اختبار" }
            };

            _mockProductRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<ISpecification<Product, int>>()))
                .ReturnsAsync(product);
            _mockMapper
                .Setup(m => m.Map<ProductDTO>(product))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            // BeEquivalentTo() for comparing DTOs (no reference equality).
            result.Value.Should().BeEquivalentTo(expectedDto, because: "The handler should return exactly what AutoMapper produces");
        }

        #endregion


        #region Failure Path Tests

        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_ProductDoesNotExist()
        {
            // Arrange
            var query = new GetProductByIdQuery(999);

            _mockProductRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<ISpecification<Product, int>>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            
            // Assert
            result.IsFailure.Should().BeTrue(because: "A non-existent product should return a failure result");
            result.Errors.Should().ContainSingle(because: "There should be exactly one 'not found' error");
            result.Errors[0].Type.Should().Be(ErrorType.NotFound);
            result.Errors[0].Code.Should().Be("Product.NotFound");
            result.Errors[0].Description.Should().Contain("999", because: "The error message should include the ID that was not found");
        }

        #endregion


        #region Behavioral Tests

        [Fact]
        public async Task Handle_Should_CallMapperWithProduct_When_ProductExists()
        {
            // Arrange
            var query = new GetProductByIdQuery(1);
            var product = new Product { Id = 1, Price = 10m, PictureUrl = "url" };

            _mockProductRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<ISpecification<Product, int>>()))
                .ReturnsAsync(product);
            _mockMapper
                .Setup(m => m.Map<ProductDTO>(product))
                .Returns(new ProductDTO { Id = 1 });
            
            // Act
            await _handler.Handle(query, CancellationToken.None);
            
            // Assert
            _mockMapper.Verify(
                m => m.Map<ProductDTO>(It.Is<Product>(p => p.Id == 1)),
                Times.Once,
                "AutoMapper should be called exactly once with the product from the repo"
            );
        }

        [Fact]
        public async Task Handle_Should_NotCallMapper_When_ProductNotFound()
        {
            // Arrange
            var query = new GetProductByIdQuery(999);
            _mockProductRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<ISpecification<Product, int>>()))
                .ReturnsAsync((Product?)null);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockMapper.Verify(
                m => m.Map<ProductDTO>(It.IsAny<Product>()),
                Times.Never,
                "AutoMapper should NOT be called when the product is not found"
            );
        }

        #endregion
    }
}
