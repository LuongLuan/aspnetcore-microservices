namespace Basket.API.Entities
{
    public class Cart
    {
        public string UserName { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public Cart() { }
        public Cart(string username)
        {
            this.UserName = username;
        }
        public decimal TotalPrice => Items.Sum(item => item.ItemPrice * item.Quantity);
    }
}

