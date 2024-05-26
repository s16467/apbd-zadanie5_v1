using Cwiczenia_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia_5.DTOs
{
    public class TripClientCountryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<int> ClientsIds { get; set; }
        public List<int> CountriesIds { get; set; }
        public List<IQueryable<Client>> Clients { get; set; }
        public List<IQueryable<Country>> Countries { get; set; }

    }

}
