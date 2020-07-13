using BookingApi.Models;

namespace BookingApi.Dtos.FlightDto
{
    public class FlightReadDto
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