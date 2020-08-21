﻿using BookingApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookingApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BookingContext context)
        {
            // Initialize airports
            InitializeAirports(context);

            // Initialize flights
            InitializeFlights(context);

            // Initialize departures
            InitializeDepartures(context);

            // Initialize destinations
            InitializeDestinations(context);

            // Initialize seats
            InitializeSeats(context);
            
            // Initialize passengers
            InitializePassengers(context);
            
            // Initialize bookings
            // InitializeBookings(context);
        }

        private static void InitializePassengers(BookingContext context)
        {
            if (context.Passengers.Any()) return;
            var passengers = PassengerList();
            
            context.Passengers.AddRange(passengers);
            context.SaveChanges();

            Console.WriteLine("Passengers data seeded");
        }

        private static void InitializeSeats(BookingContext context)
        {
            if (context.Seats.Any()) return;

            var seats = new List<Seat>()
            {
                new Seat()
                {
                    SeatNum = "1A",
                    Cabin = "Bus",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "2A",
                    Cabin = "Bus",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "3A",
                    Cabin = "Bus",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "4B",
                    Cabin = "Bus",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "5B",
                    Cabin = "Bus",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "10A",
                    Cabin = "Eco",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "21C",
                    Cabin = "Eco",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "21D",
                    Cabin = "Eco",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "11B",
                    Cabin = "Eco",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "25B",
                    Cabin = "Eco",
                    FlightId = 1
                },
                new Seat()
                {
                    SeatNum = "1A",
                    Cabin = "Bus",
                    FlightId = 3
                },
                new Seat()
                {
                    SeatNum = "2A",
                    Cabin = "Bus",
                    FlightId = 3
                },
                new Seat()
                {
                    SeatNum = "3A",
                    Cabin = "Bus",
                    FlightId = 2
                },
                new Seat()
                {
                    SeatNum = "4B",
                    Cabin = "Bus",
                    FlightId = 3
                },
                new Seat()
                {
                    SeatNum = "5B",
                    Cabin = "Bus",
                    FlightId = 2
                },
                new Seat()
                {
                    SeatNum = "10A",
                    Cabin = "Eco",
                    FlightId = 3
                },
                new Seat()
                {
                    SeatNum = "21C",
                    Cabin = "Eco",
                    FlightId = 2
                },
                new Seat()
                {
                    SeatNum = "21D",
                    Cabin = "Eco",
                    FlightId = 2
                },
                new Seat()
                {
                    SeatNum = "11B",
                    Cabin = "Eco",
                    FlightId = 3
                },
                new Seat()
                {
                    SeatNum = "25B",
                    Cabin = "Eco",
                    FlightId = 2
                }
            };

            context.Seats.AddRange(seats);
            context.SaveChanges();

            Console.WriteLine("Seats data seeded");
        }

        private static void InitializeDestinations(BookingContext context)
        {
            if (context.Destinations.Any()) return;

            var destinations = new List<Destination>()
            {
                new Destination()
                {
                    Date = new DateTime(2020, 9, 14, 20, 15, 53),
                    AirportId = 5479,
                    FlightId = 1
                },
                new Destination()
                {
                    Date = new DateTime(2020, 11, 1, 16, 15, 53),
                    AirportId = 5476,
                    FlightId = 2
                },
                new Destination()
                {
                    Date = new DateTime(2020, 12, 1, 19, 0, 53),
                    AirportId = 5381,
                    FlightId = 3
                },
            };

            context.Destinations.AddRange(destinations);
            context.SaveChanges();

            Console.WriteLine("Destinations data seeded");
        }

        private static void InitializeDepartures(BookingContext context)
        {
            if (context.Departures.Any()) return;

            var departures = new List<Departure>()
            {
                new Departure()
                {
                    Date = new DateTime(2020, 9, 14, 10, 0, 53),
                    AirportId = 5382,
                    FlightId = 1
                },
                new Departure()
                {
                    Date = new DateTime(2020, 11, 1, 10, 0, 53),
                    AirportId = 5382,
                    FlightId = 2
                },
                new Departure()
                {
                    Date = new DateTime(2020, 12, 1, 10, 0, 53),
                    AirportId = 5382,
                    FlightId = 3
                }
            };

            context.Departures.AddRange(departures);
            context.SaveChanges();

            Console.WriteLine("Departures data seeded");
        }

        private static void InitializeFlights(BookingContext context)
        {
            if (context.Flights.Any()) return;

            var flights = new List<Flight>()
            {
                new Flight()
                {
                    Name = "EA 0846",
                    Description = "New Flight, fly with Eagle Airlines",
                },
                new Flight()
                {
                    Name = "EA 0746",
                    Description = "New Flight, fly with Eagle Airlines",
                },
                new Flight()
                {
                    Name = "EA 0741",
                    Description = "New Flight, fly with Eagle Airlines",
                }
            };

            context.Flights.AddRange(flights);
            context.SaveChanges();

            Console.WriteLine("Flights data seeded");
        }

        private static void InitializeAirports(BookingContext context)
        {
            if (context.Airports.Any())
                return;

            var airports = ReadAirportJsonFile();
            context.Airports.AddRange(airports);
            context.SaveChanges();

            Console.WriteLine("Airports data seeded");
        }

        private static IEnumerable<Airport> ReadAirportJsonFile()
        {
            var jArray = JArray.Parse(File.ReadAllText(@".\Data\airports.json"));

            Console.WriteLine("Read file\n");

            return (from item in jArray
                    let name = ((string)item["name"]).Trim()
                    let city = ((string)item["city"]).Trim()
                    let country = ((string)item["country"]).Trim()
                    let iata = ((string)item["IATA"]).Trim()
                    let iciao = ((string)item["ICIAO"]).Trim()
                    let latitude = ((string)item["latitude"]).Trim()
                    let longitude = ((string)item["longitude"]).Trim()
                    let altitude = ((string)item["altitude"]).Trim()
                    let timezone = ((string)item["timezone"]).Trim()
                    let dst = ((string)item["dst"]).Trim()
                    let tz = ((string)item["tz"]).Trim()
                    let stationType = ((string)item["station_type"]).Trim()
                    let source = ((string)item["source"]).Trim()
                    select new Airport
                    {
                        Name = name,
                        City = city,
                        Country = country,
                        Iata = iata,
                        Iciao = iciao,
                        Latitude = latitude,
                        Longitude = longitude,
                        Altitude = altitude,
                        Timezone = timezone,
                        Dst = dst,
                        Tz = tz,
                        StationType = stationType,
                        Source = source
                    }).ToList();
        }
        
        
        private static IEnumerable<Passenger> PassengerList() {
            return new List<Passenger>()
            {
                new Passenger()
                {
                    Email = "christ.tchambila@eaglestack.com",
                    Name = "Chris",
                    Surname = "De-Tchambila",
                    Age = 20,
                    Title =  "Mr.",
                    PassportNumber = "OA0158375",
                    Citizenship = "South African"
                },
                new Passenger()
                {
                    Email = "melody.tchambila@eaglestack.com",
                    Name = "Melody",
                    Surname = "Tchambila",
                    Age = 18,
                    Title =  "Miss",
                    PassportNumber = "OA2196375",
                    Citizenship = "South African"
                },
                new Passenger()
                {
                    Email = "hadassah.leya@eaglestack.com",
                    Name = "Hadassah",
                    Surname = "Leya",
                    Age = 22,
                    Title =  "Mrs.",
                    PassportNumber = "DA119ZA75",
                    Citizenship = "Israeli"
                },
                new Passenger()
                {
                    Email = "david.thom@eaglestack.com",
                    Name = "David",
                    Surname = "Thom",
                    Age = 23,
                    Title =  "Mr.",
                    PassportNumber = "ZA2158375",
                    Citizenship = "South African"
                },
                new Passenger()
                {
                    Email = "rebeca.macron@eaglestack.com",
                    Name = "Rebeca",
                    Surname = "Joy",
                    Age = 18,
                    Title =  "Miss",
                    PassportNumber = "FR2996345",
                    Citizenship = "French"
                },
                new Passenger()
                {
                    Email = "kim.leea@eaglestack.com",
                    Name = "Kim",
                    Surname = "Lee",
                    Age = 25,
                    Title =  "Mrs.",
                    PassportNumber = "US139FA75",
                    Citizenship = "American"
                }
            };
        }
    }
    
}