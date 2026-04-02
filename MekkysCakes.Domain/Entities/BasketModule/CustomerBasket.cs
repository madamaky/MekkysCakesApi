namespace MekkysCakes.Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!; // Created From Client Side
        public ICollection<BasketItem> Items { get; set; } = [];
    }
}
