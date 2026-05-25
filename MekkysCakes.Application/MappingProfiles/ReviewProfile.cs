using AutoMapper;
using MekkysCakes.Application.Features.Reviews.Queries.GetAllReviews;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared.DTOs.ReviewDTOs;

namespace MekkysCakes.Application.MappingProfiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ProductReview, ProductReviewDTO>()
                .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => src.User.DisplayName));

            CreateMap<ProductReview, ReviewDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Translations
                                                                                   .Where(t => t.Language == "en")
                                                                                   .Select(t => t.Name)
                                                                                   .FirstOrDefault() ?? string.Empty))
                .ForMember(dest => dest.ProductPictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => src.User.DisplayName));
        }
    }
}
