using AutoMapper;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.BasketModule;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Services.Specifications.OrderSpecifications;
using MekkysCakes.Shared;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(string email, OrderDTO orderDTO)
        {
            // 1- Map From AddressDTO to Address Entity
            var orderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            // 2- Get Basket By Id From Basket Repository
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (basket is null)
                return Error.NotFound("Basket.NotFound", $"The Basket With Id {orderDTO.BasketId} Was Not Found");
            if (!basket.Items.Any())
                return Error.NotFound("Basket.NotFound", $"The Basket With Id {orderDTO.BasketId} Has No Products");

            // 3- Find any existing "Pending" order for this user to prevent database spam.
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var pendingOrdersSpec = new PendingOrdersSpecification(email);
            var existingOrder = await orderRepo.GetByIdAsync(pendingOrdersSpec);
            if (existingOrder != null)
                orderRepo.Delete(existingOrder);

            // 4- Get Order Items From Basket Items
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("Product.NotFound", $"The Product With Id {item.Id} Was Not Found");

                orderItems.Add(CreateOrderItem(item, product));
            }

            // 5- Get Delivery Method By Id
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"The Delivery Method With Id {orderDTO.DeliveryMethodId} Was Not Found");

            // 6- Calculate Subtotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 7- Create Order Entity
            var order = new Order
            {
                Address = orderAddress,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subTotal,
                UserEmail = email,
                PhoneNumber = "NUMBER"
            };

            //// Mark the new order for insertion.
            await orderRepo.AddAsync(order);

            // 8- ATOMIC SAVE (The Magic Transaction)
            // This executes the Delete (if it existed) and the Insert at the exact same time.
            bool result = await _unitOfWork.SaveChangesAsync();
            if (!result)
                return Error.Failure("Order.Failure", "An Error Occurred While Creating The Order");

            // 9- Clean up the user's basket now that the order is placed!
            await _basketRepository.DeleteBasketAsync(basket.Id);

            // 10- Return OrderToReturnDTO
            return _mapper.Map<OrderToReturnDTO>(order);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(string email, Guid id)
        {
            var spec = new OrderSpecification(id, email);

            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"No Orders Were Found For User With Email {email}.");

            return _mapper.Map<OrderToReturnDTO>(order);
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email)
        {
            var Spec = new OrderSpecification(email);

            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(Spec);
            if (orders is null || !orders.Any())
                return Error.NotFound("Order.NotFound", $"No Orders Were Found For User With Email {email}.");

            var Data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(Data);
        }

        public async Task<Result<PaginatedResult<OrderToReturnDTO>>> GetAllOrdersForAdminAsync(OrderQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Order, Guid>();
            var spec = new OrderSpecification(queryParams);
            var orders = await repo.GetAllAsync(spec);

            //if (orders is null || !orders.Any())
            //    return Error.NotFound("Order.NotFound", "No Orders Were Found.");

            var ordersToReturn = _mapper.Map<IEnumerable<OrderToReturnDTO>>(orders);
            var totalOrdersCount = await repo.CountAsync(new OrdersCountSpecification(queryParams));

            return new PaginatedResult<OrderToReturnDTO>
            (
                queryParams.PageIndex,
                ordersToReturn.Count(),
                await _unitOfWork.GetRepository<Order, Guid>().CountAsync(spec),
                ordersToReturn
            );
        }

        public async Task<Result<bool>> UpdateOrderStatusAsync(Guid orderId, OrderStatusDTO newStatus)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(orderId);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"Order With Id {orderId} Was Not Found.");

            if (order.OrderStatus == (OrderStatus)newStatus)
                return Error.Validation("Order.Validation", $"Order With Id {orderId} Is Already With Status {newStatus}");

            order.OrderStatus = (OrderStatus)newStatus;

            var result = await _unitOfWork.SaveChangesAsync();
            return result ? true : Error.Failure("Order.Failure", $"Failed To Update Status For Order With Id {orderId}.");
        }

        public async Task<Result<bool>> CancelOrderAsync(string email, Guid orderId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(orderId);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"Order With Id {orderId} Was Not Found.");

            if (order.UserEmail != email)
                return Error.Validation("Order.Validation", $"User With Email {email} Does Not Have Order With Id {orderId}.");

            if (order.OrderStatus == OrderStatus.Cancelled)
                return Error.Validation("Order.Validation", $"Order With Id {orderId} Was Already Cancelled.");

            if (order.OrderStatus != OrderStatus.Pending)
                return Error.Validation("Order.Validation", $"Only Pending Orders Can Be Cancelled. Order With Id {orderId} Is With Status {order.OrderStatus}.");

            order.OrderStatus = OrderStatus.Cancelled;

            var result = await _unitOfWork.SaveChangesAsync();
            return result ? true : Error.Failure("Order.Failure", $"Failed To Update Status For Order With Id {orderId}.");
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!DeliveryMethods.Any())
                return Error.NotFound("DeliveryMethod.NotFound", "No Delivery Methods Were Found.");

            var Data = _mapper.Map<IEnumerable<DeliveryMethodDTO>>(DeliveryMethods);

            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(Data);
        }

        //public async Task<Result<OrderStatsDTO>> GetOrderStatisticsAsync()
        //{
        //    var repo = _unitOfWork.GetRepository<Order, Guid>();

        //    var totalOrders = await repo.CountAsync();
        //    var totalRevenue = await repo
        //        .Where(o => o.OrderStatus == OrderStatus.Delivered)
        //        .SumAsync(o => o.SubTotal + o.DeliveryMethod.Price);

        //    var pendingOrders = await repo
        //        .CountAsync(o => o.OrderStatus == OrderStatus.Pending);

        //    var stats = new OrderStatsDTO
        //    {
        //        TotalOrders = totalOrders,
        //        TotalRevenue = totalRevenue,
        //        PendingOrders = pendingOrders
        //    };

        //    return stats;
        //}



        #region Helper Methods

        private static OrderItem CreateOrderItem(BasketItem item, Product product)
            => new OrderItem
            {
                Product = new ProductItemOrdered
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl
                },
                Price = product.Price,
                Quantity = item.Quantity
            };

        #endregion
    }
}
