﻿using BookingApi.Dtos.DepartureDto;
using BookingApi.Dtos.DestinationDto;

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

        public FlightDepartureReadDto Departure { get; set; }
        public FlightDestinationReadDto Destination { get; set; }

    }
}