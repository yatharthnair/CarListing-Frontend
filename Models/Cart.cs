namespace Car_Listing.Models
{
    public class Cart
    {
        public int cartId { get; set; }
        public string? CarName { get; set; }
        public string? rental { get; set; }
        public byte[]? image { get; set; }
        public string? imgURL { get; set; }
    }
}
