using System.ComponentModel.DataAnnotations;


namespace Car_Listing.Models
{
    public class CarDetails
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter the carname")]
        [StringLength(100, MinimumLength = 0)]
        public string? CarName { get; set; }
        [Required(ErrorMessage = "Please Enter the Mileage")]
        public float? Mileage { get; set; }
        [Required(ErrorMessage = "Please Enter the Condition")]
        public string? condition { get; set; }
        [Required(ErrorMessage = "Please Enter the Rent")]
        [Range(0, 1000)]
        public string? rental { get; set; }
        public  IFormFile? formFile { get; set; }
        public byte[]? image { get; set; }
        public string? imgURL { get; set; } 
        public string? base64img { get; set; }
    }
}
