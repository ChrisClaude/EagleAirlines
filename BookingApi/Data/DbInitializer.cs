using BookingApi.Models;
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

            // Initialize customers
            InitializeCustomers(context);
            
            // Initialize bookings
            InitializeBookings(context);
            
            // Initialize seats
            InitializeSeats(context);

            // Initialize passengers
            InitializePassengers(context);
        }

        private static void InitializeCustomers(BookingContext context)
        {
            if (context.Customers.Any()) return;

            var customers = CustomerList();
            context.Customers.AddRange(customers);
            context.SaveChanges();

            Console.WriteLine("Customers data seeded");
        }

        private static void InitializeBookings(BookingContext context)
        {
            if (context.Bookings.Any()) return;

            var bookings = BookingList();
            context.Bookings.AddRange(bookings);
            context.SaveChanges();

            Console.WriteLine("Bookings data seeded");
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

            var seats = SeatsList();

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
                let name = ((string) item["name"]).Trim()
                let city = ((string) item["city"]).Trim()
                let country = ((string) item["country"]).Trim()
                let iata = ((string) item["IATA"]).Trim()
                let iciao = ((string) item["ICIAO"]).Trim()
                let latitude = ((string) item["latitude"]).Trim()
                let longitude = ((string) item["longitude"]).Trim()
                let altitude = ((string) item["altitude"]).Trim()
                let timezone = ((string) item["timezone"]).Trim()
                let dst = ((string) item["dst"]).Trim()
                let tz = ((string) item["tz"]).Trim()
                let stationType = ((string) item["station_type"]).Trim()
                let source = ((string) item["source"]).Trim()
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

        private static IEnumerable<Passenger> PassengerList()
        {
            return new List<Passenger>()
            {
                new Passenger()
                {
                    Name = "Chris",
                    Surname = "De-Tchambila",
                    Age = 20,
                    Title = "Mr.",
                    PassportNumber = "OA0158375",
                    Citizenship = "South African",
                    BookingId = 1,
                    SeatId = 1
                },
                new Passenger()
                {
                    Name = "Melody",
                    Surname = "Tchambila",
                    Age = 18,
                    Title = "Miss",
                    PassportNumber = "OA2196375",
                    Citizenship = "South African",
                    BookingId = 1,
                    SeatId = 2
                },
                new Passenger()
                {
                    Name = "Mari",
                    Surname = "Ahskelon",
                    Age = 22,
                    Title = "Miss",
                    PassportNumber = "ISR119ZA75",
                    Citizenship = "Israeli",
                    BookingId = 2,
                    SeatId = 3
                },
                new Passenger()
                {
                    Name = "Cameron",
                    Surname = "Mosterat",
                    Age = 28,
                    Title = "Mr.",
                    PassportNumber = "AUS2158375",
                    Citizenship = "Australian",
                    BookingId = 3,
                    SeatId = 4
                },
                new Passenger()
                {
                    Name = "Ashanti",
                    Surname = "Sana",
                    Age = 17,
                    Title = "Miss",
                    PassportNumber = "KEN2996345",
                    Citizenship = "Kenyan",
                    BookingId = 3,
                    SeatId = 5
                },
                new Passenger()
                {
                    Name = "Kimberly",
                    Surname = "Asheklon",
                    Age = 25,
                    Title = "Mrs.",
                    PassportNumber = "ISR139FA75",
                    Citizenship = "Israeli",
                    BookingId = 3,
                    SeatId = 6
                },
                new Passenger()
                {
                    Name = "Christian",
                    Surname = "Baboka",
                    Age = 27,
                    Title = "Mr.",
                    PassportNumber = "ETh01584w5",
                    Citizenship = "Ethiopian",
                    BookingId = 1,
                    SeatId = 7
                },
                new Passenger()
                {
                    Name = "Elise",
                    Surname = "Tchambila",
                    Age = 23,
                    Title = "Miss",
                    PassportNumber = "OA2196375",
                    Citizenship = "South African",
                    BookingId = 2,
                    SeatId = 8
                },
                new Passenger()
                {
                    Name = "Esther",
                    Surname = "Leya",
                    Age = 29,
                    Title = "Mrs.",
                    PassportNumber = "DA119ZA75",
                    Citizenship = "Israeli",
                    BookingId = 2,
                    SeatId = 9
                },
                new Passenger()
                {
                    Name = "Davy",
                    Surname = "Thom",
                    Age = 27,
                    Title = "Mr.",
                    PassportNumber = "ZA2158895",
                    Citizenship = "South African",
                    BookingId = 1,
                    SeatId = 10
                },
                new Passenger()
                {
                    Name = "Sara",
                    Surname = "Marin",
                    Age = 18,
                    Title = "Miss",
                    PassportNumber = "FR2996345",
                    Citizenship = "French",
                    BookingId = 3,
                    SeatId = 11
                },
                new Passenger()
                {
                    Name = "Veroniqua",
                    Surname = "Loembet",
                    Age = 25,
                    Title = "Mrs.",
                    PassportNumber = "AO139FA75",
                    Citizenship = "Congolese",
                    BookingId = 3,
                    SeatId = 12 
                },
                new Passenger()
                {
                    Name = "Owin",
                    Surname = "Ntumba",
                    Age = 20,
                    Title = "Mrs.",
                    PassportNumber = "OA01Ms12",
                    Citizenship = "Congolese",
                    BookingId = 1,
                    SeatId = 13
                },
                new Passenger()
                {
                    Name = "Emmanuella",
                    Surname = "Namib",
                    Age = 18,
                    Title = "Miss",
                    PassportNumber = "Nam2196375",
                    Citizenship = "Namibian",
                    BookingId = 2,
                    SeatId = 14
                },
                new Passenger()
                {
                    Name = "Steven",
                    Surname = "Kajuki",
                    Age = 22,
                    Title = "Mr.",
                    PassportNumber = "Rw119ZA75",
                    Citizenship = "Rwadan",
                    BookingId = 2,
                    SeatId = 15
                },
                new Passenger()
                {
                    Name = "Isabella",
                    Surname = "Dos Santos",
                    Age = 32,
                    Title = "Mrs.",
                    PassportNumber = "ANG21AF8375",
                    Citizenship = "Angolese",
                    BookingId = 3,
                    SeatId = 16
                },
                new Passenger()
                {
                    Name = "Carine",
                    Surname = "Ouatara",
                    Age = 18,
                    Title = "Miss",
                    PassportNumber = "IVC2996345",
                    Citizenship = "Ivorian",
                    BookingId = 3,
                    SeatId = 17
                },
                new Passenger()
                {
                    Name = "Mel",
                    Surname = "Melis",
                    Age = 25,
                    Title = "Mrs.",
                    PassportNumber = "Fin139FA75",
                    Citizenship = "Finish",
                    BookingId = 3,
                    SeatId = 18
                },
                new Passenger()
                {
                    Name = "Rena",
                    Surname = "Pen",
                    Age = 23,
                    Title = "Miss",
                    PassportNumber = "ENG2996345",
                    Citizenship = "English",
                    BookingId = 3,
                    SeatId = 19
                },
                new Passenger()
                {
                    Name = "Karim",
                    Surname = "Lee",
                    Age = 38,
                    Title = "Mrs.",
                    PassportNumber = "US139FA78",
                    Citizenship = "American",
                    BookingId = 3,
                    SeatId = 20 
                }
            };
        }

        private static IEnumerable<Booking> BookingList()
        {
            return new List<Booking>()
            {
                new Booking()
                {
                    Cost = new decimal(2500),
                    Status = "Confirmed",
                    CustomerId = 1
                },
                new Booking()
                {
                    Cost = new decimal(7500),
                    Status = "Not Confirmed",
                    CustomerId = 2
                },
                new Booking()
                {
                    Cost = new decimal(9500),
                    Status = "Not Confirmed",
                    CustomerId = 3
                }
            };
        }

        private static IEnumerable<Seat> SeatsList()
        {
            return new List<Seat>()
            {
                new Seat()
                {
                    SeatNum = "1A",
                    Cabin = "Bus",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "2A",
                    Cabin = "Bus",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "3A",
                    Cabin = "Bus",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "4B",
                    Cabin = "Bus",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "5B",
                    Cabin = "Bus",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "10A",
                    Cabin = "Eco",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "21C",
                    Cabin = "Eco",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "21D",
                    Cabin = "Eco",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "11B",
                    Cabin = "Eco",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "25B",
                    Cabin = "Eco",
                    FlightId = 1,
                },
                new Seat()
                {
                    SeatNum = "1A",
                    Cabin = "Bus",
                    FlightId = 3,
                },
                new Seat()
                {
                    SeatNum = "2A",
                    Cabin = "Bus",
                    FlightId = 3,
                },
                new Seat()
                {
                    SeatNum = "3A",
                    Cabin = "Bus",
                    FlightId = 2,
                },
                new Seat()
                {
                    SeatNum = "4B",
                    Cabin = "Bus",
                    FlightId = 3,
                },
                new Seat()
                {
                    SeatNum = "5B",
                    Cabin = "Bus",
                    FlightId = 2,
                },
                new Seat()
                {
                    SeatNum = "10A",
                    Cabin = "Eco",
                    FlightId = 3,
                },
                new Seat()
                {
                    SeatNum = "21C",
                    Cabin = "Eco",
                    FlightId = 2,
                },
                new Seat()
                {
                    SeatNum = "21D",
                    Cabin = "Eco",
                    FlightId = 2,
                },
                new Seat()
                {
                    SeatNum = "11B",
                    Cabin = "Eco",
                    FlightId = 3,
                },
                new Seat()
                {
                    SeatNum = "25B",
                    Cabin = "Eco",
                    FlightId = 2,
                }
            };
        }

        private static IEnumerable<Customer> CustomerList()
        {
            return new List<Customer>()
            {
                new Customer()
                {
                    Name = "Chris",
                    Email = "christ.eaglestack.com",
                    Address = "29 Hadley St Brackenfell, Cape Town"
                },
                new Customer()
                {
                    Name = "Martin",
                    Email = "martin.eaglestack.com",
                    Address = "12 Kempstone St Sandton, Johannesburg"
                },
                new Customer()
                {
                    Name = "Olivier",
                    Email = "olivier.gmail.com",
                    Address = "15 Briardene St Tygerberg, Cape Town"
                },
            };
        }
    }
}