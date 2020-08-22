using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingApi.Dtos.CustomerDto;
using BookingApi.Dtos.FlightDto;
using BookingApi.Dtos.PassengerDto;
using BookingApi.Dtos.SeatDto;
using BookingApi.Models;

namespace BookingApi.Dtos.BookingDto
{
    public class BookingReadDto
    {
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public decimal Cost { get; set; }
        public string Status { get; set; }
        public int PassengerId { get; set; }
        public int CustomerId { get; set; }


        public IEnumerable<PassengerReadDto> Passengers { get; set; }
        public BookingCustomerReadDto Customer { get; set; }

    }
}