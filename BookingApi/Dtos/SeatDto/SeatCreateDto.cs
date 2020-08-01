﻿using BookingApi.Models;

namespace BookingApi.Dtos.SeatDto
{
    public class SeatCreateDto
    {
        public int ID { get; set; }
        public string SeatNum { get; set; }
        public Cabin Cabin { get; set; }
        public int FlightID { get; set; }
    }
}