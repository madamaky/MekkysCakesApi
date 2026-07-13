using System.Xml.Linq;
using AutoMapper;
using FluentAssertions;
using MekkysCakes.Application.Features.Products.Commands.CreateProduct;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;
using Moq;

namespace MekkysCakes.Tests.Application.Handlers
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;

        private readonly Mock<IGenericRepository<Product, int>> _mockProductRepo;
        private readonly Mock<IGenericRepository<ProductType, int>> _mockProductTypeRepo;
        private readonly Mock<IGenericRepository<ProductTheme, int>> _mockProductThemeRepo;
        private readonly Mock<IGenericRepository<Badge, int>> _mockBadgeRepo;

        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            // Create all mocks
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockProductRepo = new Mock<IGenericRepository<Product, int>>();
            _mockProductTypeRepo = new Mock<IGenericRepository<ProductType, int>>();
            _mockProductThemeRepo = new Mock<IGenericRepository<ProductTheme, int>>();
            _mockBadgeRepo = new Mock<IGenericRepository<Badge, int>>();

            // Wire up IUnitOfWork.GetRepository<T, TKey>() to return mock repos
            _mockUnitOfWork
                .Setup(uow => uow.GetRepository<Product, int>())
                .Returns(_mockProductRepo.Object);
            _mockUnitOfWork
                .Setup(uow => uow.GetRepository<ProductType, int>())
                .Returns(_mockProductTypeRepo.Object);
            _mockUnitOfWork
                .Setup(uow => uow.GetRepository<ProductTheme, int>())
                .Returns(_mockProductThemeRepo.Object);
            _mockUnitOfWork
                .Setup(uow => uow.GetRepository<Badge, int>())
                .Returns(_mockBadgeRepo.Object);

            // Create the handler with mocked dependencies
            _handler = new CreateProductCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region Success Path Tests

        [Fact]
        public async Task Handle_Should_ReturnSuccess_When_AllEntitiesExist()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Chocolate Cake", Ar = "كيكة شوكولاتة" },
                Description: new LocalizedString { En = "Delicious", Ar = "لذيذة" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 29.99m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1, 2]
            );

            _mockProductTypeRepo
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ProductType { Id = 1 });
            _mockProductThemeRepo
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ProductTheme { Id = 1 });

            _mockBadgeRepo
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Badge { Id = 1 });
            _mockBadgeRepo
                .Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(new Badge { Id = 2 });

            _mockMapper
                .Setup(mapper => mapper.Map<Product>(It.IsAny<CreateProductCommand>()))
                .Returns(new Product
                {
                    Price = command.Price,
                    PictureUrl = command.PictureUrl,
                    TypeId = command.TypeId,
                    ThemeId = command.ThemeId
                });

            _mockUnitOfWork
                .Setup(uow => uow.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue(because: "All related entities exist, so the product should be created successfully");
            result.Value.Should().BeTrue(because: "SaveChangesAsync returned true");
        }

        [Fact]
        public async Task Handle_Should_CallAddAsync_When_AllEntitiesExist()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            _mockProductTypeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductType { Id = 1 });
            _mockProductThemeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductTheme { Id = 1 });
            _mockBadgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Badge { Id = 1 });
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductCommand>())).Returns(new Product());
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockProductRepo
                .Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once, "AddAsync() should be called once when all entities exist");

            _mockUnitOfWork
                .Verify(uow => uow.SaveChangesAsync(), Times.Once, "SaveChangesAsync() should be called once after adding the product");
        }

        [Fact]
        public async Task Handle_Should_CreateCorrectTranslations_When_CommandIsValid()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Chocolate Cake", Ar = "كيكة شوكولاتة" },
                Description: new LocalizedString { En = "Delicious", Ar = "لذيذة" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 29.99m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            _mockProductTypeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductType { Id = 1 });
            _mockProductThemeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductTheme { Id = 1 });
            _mockBadgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Badge { Id = 1 });

            Product? capturedProduct = null;

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductCommand>())).Returns(new Product());
            _mockProductRepo
                .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p);
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedProduct.Should().NotBeNull("The handler should have called AddAsync with a product");
            capturedProduct!.Translations.Should().HaveCount(2, because: "The handler creates English and Arabic translations");
            capturedProduct.Translations.Should().Contain(t => t.Language == "en" && t.Name == "Chocolate Cake",
                because: "the English translation should use the command's English name");
            capturedProduct.Translations.Should().Contain(t => t.Language == "ar" && t.Name == "كيكة شوكولاتة",
                because: "the Arabic translation should use the command's Arabic name");
        }

        #endregion


        #region Failure Path Tests — Entity Not Found

        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_ProductTypeDoesNotExist()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 999,
                ThemeId: 1,
                BadgeIds: [1]
            );

            _mockProductTypeRepo
                .Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((ProductType?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue(because: "The product type does not exist so the Handler fails");
            result.Errors.Should().ContainSingle("The handler should fail when the ProductType is not found");
            result.Errors[0].Type.Should().Be(ErrorType.NotFound, "A missing entity should produce a NotFound error");
            result.Errors[0].Code.Should().Be("ProductType.NotFound", "The error code should identify what entity was not found");
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_ProductThemeDoesNotExist()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 999,
                BadgeIds: [1]
            );

            _mockProductTypeRepo
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new ProductType { Id = 1 });

            _mockProductThemeRepo
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((ProductTheme?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorType.NotFound);
            result.Errors[0].Code.Should().Be("ProductTheme.NotFound");
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_BadgeDoesNotExist()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1, 999]
            );

            _mockProductTypeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductType { Id = 1 });
            _mockProductThemeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductTheme { Id = 1 });

            _mockBadgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Badge { Id = 1 });
            _mockBadgeRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Badge?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Errors[0].Type.Should().Be(ErrorType.NotFound);
            result.Errors[0].Code.Should().Be("Badge.NotFound");
        }

        #endregion


        #region Behavioral Verification Tests

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_When_ProductTypeNotFound()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 999,
                ThemeId: 1,
                BadgeIds: [1]
            );

            _mockProductTypeRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ProductType?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockProductRepo.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never, "AddAsync() should NOT be called when ProductType doesn't exist");
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never, "SaveChangesAsync() should NOT be called when validation fails");
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChanges_When_ThemeNotFound()
        {
            // Arrange
            var command = new CreateProductCommand(
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 999,
                BadgeIds: [1]
            );

            _mockProductTypeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductType { Id = 1 });
            _mockProductThemeRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ProductTheme?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never, "SaveChangesAsync() should NOT be called when Theme doesn't exist");
        }

        #endregion


        #region Edge Case Tests

        [Fact]
        public async Task Handle_Should_HandleDuplicateBadgeIds_When_BadgesListHasDuplicates()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1, 1, 2]
            );

            _mockProductTypeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductType { Id = 1 });
            _mockProductThemeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductTheme { Id = 1 });
            _mockBadgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Badge { Id = 1 });
            _mockBadgeRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new Badge { Id = 2 });
            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductCommand>())).Returns(new Product());
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(true);

            Product? capturedProduct = null;
            _mockProductRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).Callback<Product>(p => capturedProduct = p);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            capturedProduct!.ProductBadges.Should().HaveCount(2, "Duplicate badge IDs should be deduplicated via .Distinct()");
        }

        #endregion
    }
}
