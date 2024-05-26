using Cwiczenia_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia_5.DTOs
{
    public class TripDTO
    {
        public int IdTrip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public ICollection<ClientTrip> ClientTrips {get; set;}
        public ICollection<CountryTrip> CountryTrips { get; set; }


    }
}
