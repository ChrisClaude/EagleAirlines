using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookingApi.Models
{
    public class Departure
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightId { get; set; }
        [JsonIgnore]
        public int AirportId { get; set; }

        [JsonIgnore]
        public Flight Flight { get; set; }
        public Airport Airport { get; set; }
    }
}
