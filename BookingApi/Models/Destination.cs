using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookingApi.Models
{
    public class Destination
    {
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightID { get; set; }
        [JsonIgnore]
        public int AirportID { get; set; }

        [JsonIgnore]
        public Flight Flight { get; set; }
        public Airport Airport { get; set; }
    }
}
