using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Models
{
    public class Airport
    {
        public int ID { get; set; }
        
        [Required]
        [StringLength(150, MinimumLength = 4)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")] // this requires the first character to be upper case and the remaining characters to be alphabetical
        public string Name { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 4)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string City { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 4)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string Country { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Iata { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Iciao { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Latitude { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Longitude { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Altitude { get; set; }
        public string Timezone { get; set; }
        public string Dst { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Tz { get; set; }
        public string StationType { get; set; }
        public string Source { get; set; }
    }
}
