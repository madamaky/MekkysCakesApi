using AutoMapper;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.DTOs.ProductDTOs;
using Microsoft.Extensions.Configuration;

namespace MekkysCakes.Services.MappingProfiles
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;

            if (source.PictureUrl.StartsWith("http"))
                return source.PictureUrl;

            var baseUrl = _configuration.GetSection("URLs")["BaseURL"];
            if (string.IsNullOrEmpty(baseUrl))
                return string.Empty;

            return $"{baseUrl}{source.PictureUrl}";
        }
    }
}
