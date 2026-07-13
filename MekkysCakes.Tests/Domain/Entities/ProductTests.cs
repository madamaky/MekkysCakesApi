using FluentAssertions;
using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Tests.Domain.Entities
{
    public class ProductTests
    {
        #region IncludeReviewInRatings Tests

        [Fact]
        public void IncludeReviewInRatings_Should_SetAverageToRating_When_FirstReview()
        {
            // Arrange
            var product = new Product();
            var rating = 4;

            // Act
            product.IncludeReviewInRatings(rating);

            // Assert
            product.TotalReviews.Should().Be(1, because: "Adding the first review should increment TotalReviews from 0 to 1");
            product.AverageRating.Should().Be(rating, because: "The first review's rating becomes the average");
        }

        [Fact]
        public void IncludeReviewInRatings_Should_CalculateCorrectAverage_When_MultipleReviews()
        {
            // Arrange
            var product = new Product();

            // Act
            product.IncludeReviewInRatings(5); //  5 / 1          = 5.0
            product.IncludeReviewInRatings(3); // (5 + 3) / 2     = 4.0
            product.IncludeReviewInRatings(4); // (5 + 3 + 4) / 3 = 4.0

            // Assert
            product.TotalReviews.Should().Be(3);
            product.AverageRating.Should().BeApproximately(4.0m, 0.01m, because: "average of [5, 3, 4] should be 4.0");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void IncludeReviewInRatings_Should_AlwaysIncrementTotalReviews_When_Called(int rating)
        {
            // Arrange
            var product = new Product();

            // Act
            product.IncludeReviewInRatings(rating);

            // Assert
            product.TotalReviews.Should().Be(1, because: "TotalReviews should increment by 1 regardless of rating value");
        }

        #endregion

        #region ExcludeReviewFromRatings Tests

        [Fact]
        public void ExcludeReviewFromRatings_Should_DoNothing_When_NoReviewsExist()
        {
            // Arrange
            var product = new Product();

            // Act
            product.ExcludeReviewFromRatings(5);

            // Assert
            product.TotalReviews.Should().Be(0, because: "You can't remove a review when there are no reviews");
            product.AverageRating.Should().Be(0, because: "Average should remain 0 when there are no reviews");
        }

        [Fact]
        public void ExcludeReviewFromRatings_Should_ResetToZero_When_LastReviewRemoved()
        {
            // Arrange
            var product = new Product();
            product.IncludeReviewInRatings(5); // TotalReviews = 1, AverageRating = 5.0

            // Act
            product.ExcludeReviewFromRatings(5);

            // Assert
            product.TotalReviews.Should().Be(0);
            product.AverageRating.Should().Be(0);
        }

        [Fact]
        public void ExcludeReviewFromRatings_Should_RecalculateAverage_When_ReviewRemoved()
        {
            // Arrange
            var product = new Product();
            product.IncludeReviewInRatings(5); // avg=5, count=1
            product.IncludeReviewInRatings(3); // avg=4, count=2
            product.IncludeReviewInRatings(4); // avg=4, count=3 

            // Act
            product.ExcludeReviewFromRatings(3);

            // Assert
            product.TotalReviews.Should().Be(2);
            product.AverageRating.Should().BeApproximately(4.5m, 0.01m, because: "Removing rating 3 from [5,3,4] leaves [5,4] → average 4.5");
        }

        #endregion

        #region UpdateReviewRating Tests

        [Fact]
        public void UpdateReviewRating_Should_DoNothing_When_NoReviewsExist()
        {
            // Arrange
            var product = new Product();

            // Act
            product.UpdateReviewRating(oldRating: 3, newRating: 5);

            // Assert
            product.TotalReviews.Should().Be(0);
            product.AverageRating.Should().Be(0);
        }

        [Fact]
        public void UpdateReviewRating_Should_UpdateAverage_When_RatingChanged()
        {
            // Arrange
            var product = new Product();
            product.IncludeReviewInRatings(5); // avg=5, count=1
            product.IncludeReviewInRatings(3); // avg=4, count=2

            // Act
            product.UpdateReviewRating(oldRating: 3, newRating: 5);

            // Assert
            product.TotalReviews.Should().Be(2, because: "updating a rating does NOT change the review count");
            product.AverageRating.Should().BeApproximately(5.0m, 0.01m, because: "changing [5,3] to [5,5] gives an average of 5.0");
        }

        [Fact]
        public void UpdateReviewRating_Should_NotChangeTotalReviews_When_RatingUpdated()
        {
            // Arrange
            var product = new Product();
            product.IncludeReviewInRatings(4);
            product.IncludeReviewInRatings(2);
            var originalCount = product.TotalReviews; // should be 2

            // Act
            product.UpdateReviewRating(oldRating: 2, newRating: 5);

            // Assert
            product.TotalReviews.Should().Be(originalCount, because: "updating a rating should never change the total review count");
        }

        #endregion
    }
}
