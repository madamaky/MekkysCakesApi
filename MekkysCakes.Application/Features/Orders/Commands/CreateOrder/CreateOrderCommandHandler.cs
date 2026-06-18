using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.BasketModule;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Application.Specifications.OrderSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Application.Specifications.ProductSpecifications;

namespace MekkysCakes.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderToReturnDTO>>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IBasketRepository basketRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<OrderToReturnDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // 1-
            // 2- Get Basket By Id From Basket Repository
            var userEmail = _currentUserService.Email;
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userId))
                return Error.Unauthorized("User.Unauthorized", "You Must Be Logged In To Place An Order");
            var basket = await _basketRepository.GetBasketAsync(userEmail);
            if (basket is null)
                return Error.NotFound("Basket.NotFound", $"The Basket For User {userEmail} Was Not Found");
            if (!basket.Items.Any())
                return Error.NotFound("Basket.NotFound", $"The Basket For User {userEmail} Has No Products");

            // 3- Find any existing "Pending" order for this user to prevent database spam.
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var pendingOrdersSpec = new PendingOrdersSpecification(userEmail);
            var existingOrder = await orderRepo.GetByIdAsync(pendingOrdersSpec);
            if (existingOrder != null)
                orderRepo.Delete(existingOrder);

            // 4- Get Order Items From Basket Items
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var spec = new ProductWithTypeAndThemeSpecification(item.Id);
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);
                if (product is null)
                    return Error.NotFound("Product.NotFound", $"The Product With Id {item.Id} Was Not Found");

                orderItems.Add(CreateOrderItem(item, product));
            }

            // 5- Get Delivery Method By Id
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(request.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"The Delivery Method With Id {request.DeliveryMethodId} Was Not Found");

            // 6- Calculate Subtotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 7- Create Order Entity
            var order = new Order
            {
                UserId = userId,
                UserName = request.UserName,
                ContactEmail = request.ContactEmail,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subTotal
            };

            // Mark the new order for insertion.
            await orderRepo.AddAsync(order);

            // 8- ATOMIC SAVE (The Magic Transaction)
            bool result = await _unitOfWork.SaveChangesAsync();
            if (!result)
                return Error.Failure("Order.Failure", "An Error Occurred While Creating The Order");

            // 9- Clean up the user's basket now that the order is placed!
            await _basketRepository.DeleteBasketAsync(basket.Id);

            // 10- Return OrderToReturnDTO
            return _mapper.Map<OrderToReturnDTO>(order);
        }

        private static OrderItem CreateOrderItem(BasketItem item, Product product)
            => new OrderItem
            {
                Product = new ProductItemOrdered
                {
                    ProductId = product.Id,
                    ProductName = product.Translations.FirstOrDefault()?.Name ?? "",
                    PictureUrl = product.PictureUrl
                },
                Price = product.Price,
                Quantity = item.Quantity
            };
    }
}
