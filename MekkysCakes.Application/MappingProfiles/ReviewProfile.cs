using AutoMapper;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared.DTOs.ReviewDTOs;

namespace MekkysCakes.Application.MappingProfiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ProductReview, ReviewDTO>()
                .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => src.User.DisplayName));
        }
    }
}
