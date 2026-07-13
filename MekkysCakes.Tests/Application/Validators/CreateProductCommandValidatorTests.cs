using FluentAssertions;
using FluentValidation.TestHelper;
using MekkysCakes.Application.Features.Products.Commands.CreateProduct;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Tests.Application.Validators
{
    public class CreateProductCommandValidatorTests
    {
        private readonly CreateProductCommandValidator _validator;

        public CreateProductCommandValidatorTests()
        {
            // xUnit creates a NEW instance of the test class for EACH test method.
            // So this constructor runs before every single test — giving each test a fresh validator.
            // This is different from NUnit/MSTest where the class instance is reused!
            _validator = new CreateProductCommandValidator();
        }

        #region Valid Command Tests

        [Fact]
        public void Validate_Should_Pass_When_CommandIsValid()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Chocolate Cake", Ar = "كيكة شوكولاتة" },
                Description: new LocalizedString { En = "Delicious chocolate cake", Ar = "كيكة شوكولاتة لذيذة" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 29.99m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1, 2]
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        #endregion

        #region Name Validation Tests

        [Fact]
        public void Validate_Should_Fail_When_NameIsNull()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: null!,
                Description: new LocalizedString { En = "Description", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );
            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_Should_Fail_When_EnglishNameIsEmpty()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );
            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name.En);
        }

        [Fact]
        public void Validate_Should_Fail_When_ArabicNameIsEmpty()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );
            // Act

            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name.Ar);
        }

        [Fact]
        public void Validate_Should_Fail_When_EnglishNameExceedsMaxLength()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = new string('A', 101), Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name.En);
        }

        #endregion

        #region Price Validation Tests

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_Should_Fail_When_PriceIsInvalid(decimal invalidPrice)
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: invalidPrice,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(1)]
        [InlineData(999.99)]
        public void Validate_Should_Pass_When_PriceIsValid(decimal validPrice)
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: validPrice,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            // Act
            var result = _validator.TestValidate(command);
            
            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        #endregion

        #region TypeId and ThemeId Validation Tests

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-50)]
        public void Validate_Should_Fail_When_TypeIdIsInvalid(int invalidTypeId)
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: invalidTypeId,
                ThemeId: 1,
                BadgeIds: [1]
            );

            // Act
            var result = _validator.TestValidate(command);
            
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TypeId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-50)]
        public void Validate_Should_Fail_When_ThemeIdIsInvalid(int invalidThemeId)
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: invalidThemeId,
                BadgeIds: [1]
            );
            
            // Act
            var result = _validator.TestValidate(command);
            
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ThemeId);
        }

        #endregion

        #region BadgeIds Validation Tests

        [Fact]
        public void Validate_Should_Fail_When_BadgeIdsIsNull()
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
                BadgeIds: null!
            );

            // Act
            var result = _validator.TestValidate(command);
           
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BadgeIds);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_Should_Fail_When_BadgeIdIsNotPositive(int invalidBadgeId)
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
                BadgeIds: [invalidBadgeId]
            );

            // Act
            var result = _validator.TestValidate(command);
            
            // Assert
            result.IsValid.Should().BeFalse(because: $"badge ID {invalidBadgeId} is not a positive integer"); // Can't use ShouldHaveValidationErrorFor with indexer for RuleForEach, so we check that the overall result has errors
        }

        #endregion

        #region PictureUrl Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_Should_Fail_When_PictureUrlIsEmpty(string? invalidUrl)
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = "Desc", Ar = "وصف" },
                PictureUrl: invalidUrl!,
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            // Act
            var result = _validator.TestValidate(command);
            
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PictureUrl);
        }

        #endregion

        #region Description Validation Tests

        [Fact]
        public void Validate_Should_Fail_When_DescriptionIsNull()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: null!,
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Validate_Should_Fail_When_EnglishDescriptionExceedsMaxLength()
        {
            // Arrange
            var command = new CreateProductCommand
            (
                Name: new LocalizedString { En = "Cake", Ar = "كيكة" },
                Description: new LocalizedString { En = new string('A', 501), Ar = "وصف" },
                PictureUrl: "https://example.com/cake.jpg",
                Price: 10m,
                TypeId: 1,
                ThemeId: 1,
                BadgeIds: [1]
            );

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description.En);
        }

        #endregion
    }
}
