using BookingApi.Dtos.DepartureDto;
using BookingApi.Dtos.DestinationDto;
using BookingApi.Models;

namespace BookingApi.Dtos.FlightDto
{
    public class FlightReadDto
    {
        public int Id { get; set; }
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

        public DepartureReadDto Departure { get; set; }
        public DestinationReadDto Destination { get; set; }

    }
}