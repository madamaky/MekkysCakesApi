namespace MekkysCakes.Shared.DTOs.OrderDTOs
{
    public record OrderDTO(string BasketId, int DeliveryMethodId, AddressDTO Address);
}
