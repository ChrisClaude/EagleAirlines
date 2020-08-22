using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.PassengerDto
{
    public class PassengerReadDto
    {
        public int Id { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Email { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Surname { get; set; }
        public int Age { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string PassportNumber { get; set; }
        
        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Citizenship { get; set; }
        public int BookingId { get; set; }
        public int SeatId { get; set; }
    }
}