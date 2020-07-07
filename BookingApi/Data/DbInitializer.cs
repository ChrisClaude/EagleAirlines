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
                string name = ((string)item["name"]).Trim();
                string city = ((string)item["city"]).Trim();
                string country = ((string)item["country"]).Trim();
                string iata = ((string)item["IATA"]).Trim();
                string iciao = ((string)item["ICIAO"]).Trim();
                string latitude = ((string)item["latitude"]).Trim();
                string longitude = ((string)item["longitude"]).Trim();
                string altitude = ((string)item["altitude"]).Trim();
                string timezone = ((string)item["timezone"]).Trim();
                string dst = ((string)item["dst"]).Trim();
                string tz = ((string)item["tz"]).Trim();
                string stationType = ((string)item["station_type"]).Trim();
                string source = ((string)item["source"]).Trim();

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
