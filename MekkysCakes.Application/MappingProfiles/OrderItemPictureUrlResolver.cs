using AutoMapper;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using Microsoft.Extensions.Configuration;

namespace MekkysCakes.Application.MappingProfiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.Product.PictureUrl))
                return string.Empty;

            if (source.Product.PictureUrl.StartsWith("http"))
                return source.Product.PictureUrl;

            var baseUrl = _configuration.GetSection("URLs")["BaseURL"];
            if (string.IsNullOrEmpty(baseUrl))
                return string.Empty;

            return $"{baseUrl}{source.Product.PictureUrl}";
        }
    }
}
