using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Models
{
    public class Passenger
    {
        public int ID { get; set; }
        [DataType(DataType.EmailAddress)]

        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Email { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 4)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
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
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string Citizenship { get; set; }
    }
}
