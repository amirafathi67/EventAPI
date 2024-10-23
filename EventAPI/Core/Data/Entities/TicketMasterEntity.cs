using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAPI.Core.Data.Entities
{
    public class Image
    {
        public string ratio { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool fallback { get; set; }
    }

    public class Sales
    {
        public PublicSale @public { get; set; }
        public List<Presale> presales { get; set; }
    }

    public class PublicSale
    {
        public DateTime startDateTime { get; set; }
        public bool startTBD { get; set; }
        public bool startTBA { get; set; }
        public DateTime endDateTime { get; set; }
    }

    public class Presale
    {
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string name { get; set; }
    }

    public class Start
    {
        public string localDate { get; set; }
        public string localTime { get; set; }
        public DateTime dateTime { get; set; }
        public bool dateTBD { get; set; }
        public bool dateTBA { get; set; }
        public bool timeTBA { get; set; }
        public bool noSpecificTime { get; set; }
    }

    public class Dates
    {
        public Start start { get; set; }
        public string timezone { get; set; }
        public Status status { get; set; }
        public bool spanMultipleDays { get; set; }
    }

    public class Status
    {
        public string code { get; set; }
    }

    public class Classification
    {
        public bool primary { get; set; }
        public Segment segment { get; set; }
        public Genre genre { get; set; }
        public SubGenre subGenre { get; set; }
        public Type type { get; set; }
        public SubType subType { get; set; }
        public bool family { get; set; }
    }

    public class Segment
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Genre
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class SubGenre
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class SubType
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class PriceRange
    {
        public string type { get; set; }
        public string currency { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }

    public class Venue
    {
        public string name { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public string locale { get; set; }
        public List<Image> images { get; set; }
        public string postalCode { get; set; }
        public string timezone { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Country country { get; set; }
        public Address address { get; set; }
        public Location location { get; set; }
    }

    public class City
    {
        public string name { get; set; }
    }

    public class State
    {
        public string name { get; set; }
        public string stateCode { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string countryCode { get; set; }
    }

    public class Address
    {
        public string line1 { get; set; }
    }

    public class Location
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
    }

    public class Event
    {
        public string name { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public List<Image> images { get; set; }
        public Sales sales { get; set; }
        public Dates dates { get; set; }
        public List<Classification> classifications { get; set; }
        public List<Venue> venues { get; set; }
        public List<PriceRange> priceRanges { get; set; }
    }

    public class TicketMasterResponse
    {
        public Embedded _embedded { get; set; }
    }

    public class Embedded
    {
        public List<Event> events { get; set; }
    }

}
