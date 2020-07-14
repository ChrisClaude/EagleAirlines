using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Models
{
    public class Flight
    {
        public int ID { get; set; }
        public double Miles 
        { 
            get 
            { 
                return 0.0; 
            } 
        }

        public int Duration {  
            get 
            {
                return 0;
            } 
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public Departure Departure { get; set; }
        public Destination Destination { get; set; }

    }
}
