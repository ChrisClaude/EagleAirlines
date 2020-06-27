using BookingApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(BookingContext context)
        {
            if (context.Airports.Any())
            {
                return;
            }

            List<Airport> airports = readAiportJsonFile();
            context.Airports.AddRange(airports);
            context.SaveChanges();
        }

        public static List<Airport> readAiportJsonFile()
        {
            List<Airport> airports = new List<Airport>();

            JArray jArray = JArray.Parse(File.ReadAllText(@".\Data\airports.json"));


            Console.WriteLine("Read file\n");

            int i = 0;

            foreach (var item in jArray)
            {
                string name = (String)item["name"];
                string city = (String)item["city"];
                string country = (String)item["country"];
                string iata = (String)item["IATA"];
                string iciao = (String)item["ICIAO"];
                string latitude = (String)item["latitude"];
                string longitude = (String)item["longitude"];
                string altitude = (String)item["altitude"];
                string timezone = (String)item["timezone"];
                string dst = (String)item["dst"];
                string tz = (String)item["tz"];
                string stationType = (String)item["station_type"];
                string source = (String)item["source"];

                airports.Add(new Airport
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
                });

                Console.WriteLine(++i);
            }

            return airports;
        }
    }
}
