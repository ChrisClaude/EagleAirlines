using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.PassengerDto
{
    public class PassengerUpdateDto
    {
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
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string PassportNumber { get; set; }
        
        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Citizenship { get; set; }

    }
}