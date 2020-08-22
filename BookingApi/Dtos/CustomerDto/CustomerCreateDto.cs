using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.CustomerDto
{
    public class CustomerCreateDto
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "The Address field is required.")]
        [StringLength(400, MinimumLength = 10)]
        public string Address { get; set; }
    }
}