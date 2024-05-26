using Cwiczenia_5.Models;
using Cwiczenia_5.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : Controller
    {

        /*
         * 
        [HttpGet]
        public IActionResult getTrips()
        {
            var context = new s16467Context();
            var trips = context.Trips.OrderBy(t => t.DateFrom);
            return Ok(trips);
        }
         * 
        */

        // The above version was what I had before and it was working...
        // I tried to actually connect the other entities (Country and Client) but for some reason sometimes
        // it gives me results sometimes it doesn't... I don't know what's wrong (in the version below - Not working)
        // I'll be happy to see how to actually make this work correctly, thank you.


        [HttpGet]
        public IActionResult getTrips()
        {
            var context = new s16467Context();
            //var trips = context.Trips.OrderBy(t => t.DateFrom).Include().Include(CountryTrip);
            List<Trip> tripsDTOs = new List<Trip>();

            IQueryable<Trip> ITripDTOs = context.Trips.Select(tt => new Trip
            {
                IdTrip = tt.IdTrip,
                Name = tt.Name,
                Description = tt.Description,
                DateFrom = tt.DateFrom,
                DateTo = tt.DateTo,
                MaxPeople = tt.MaxPeople,
                ClientTrips = tt.ClientTrips,
                CountryTrips = tt.CountryTrips
            });

            List<TripClientCountryDTO> tripClientCountryDTOs = new List<TripClientCountryDTO>();

            Console.WriteLine("============: " + tripsDTOs.Count() + " TripsDTOs: " + tripsDTOs);
            Console.WriteLine("============: " + tripClientCountryDTOs.Count() + " tripClientCountryDTOs: " + tripClientCountryDTOs);
            ITripDTOs.ForEachAsync(tDTO =>
            {
                TripClientCountryDTO tcc = new TripClientCountryDTO
                {
                    Name = tDTO.Name,
                    Description = tDTO.Description,
                    DateFrom = tDTO.DateFrom,
                    DateTo = tDTO.DateTo,
                    MaxPeople = tDTO.MaxPeople,
                    ClientsIds = new List<int>(),
                    CountriesIds = new List<int>()
                    
                };
                tDTO.ClientTrips.ToList().ForEach(ct =>
                {
                    tcc.ClientsIds.Add(ct.IdClient);
                });
                tDTO.CountryTrips.ToList().ForEach(ct =>
                {
                    tcc.CountriesIds.Add(ct.IdCountry);
                });
                tripClientCountryDTOs.Add(tcc);
            });
            Console.WriteLine("============: " + tripsDTOs.Count() + " TripsDTOs: " + tripsDTOs);
            Console.WriteLine("============: " + tripClientCountryDTOs.Count() + " tripClientCountryDTOs: " + tripClientCountryDTOs);

            tripClientCountryDTOs.ForEach(tccDTO =>
            {
                tccDTO.Clients = new List<IQueryable<Client>>();
                tccDTO.ClientsIds.ForEach(ci =>
                {
                    tccDTO.Clients.Add(context.Clients.Where(c => c.IdClient.Equals(ci)));
                });
                tccDTO.Countries = new List<IQueryable<Country>>();
                tccDTO.CountriesIds.ForEach(cci =>
                {
                    tccDTO.Countries.Add(context.Countries.Where(c => c.IdCountry.Equals(cci)));
                });
            });

            Console.WriteLine("============: " + tripsDTOs.Count() + " TripsDTOs: " + tripsDTOs);
            Console.WriteLine("============: " + tripClientCountryDTOs.Count() + " tripClientCountryDTOs: " + tripClientCountryDTOs);
            return Ok(tripClientCountryDTOs);
        }

        [HttpPost("{idTrip}/clients")]
        public IActionResult assignClientToTrip(int idTrip, [FromBody] ClientTripAssignDTO clientTripAssignDTO)
        {
            var context = new s16467Context();

            // czy wycieczka istnieje
            if (context.Trips
                .Where(t => t.IdTrip.Equals(idTrip))
                .Count() == 0)
            {
                return NotFound("Trip with the given id does not exist");
            }
            
            // czy klient istnieje, jeśli nie to utwórz nowy klient i dodaj do contextu
            if (context.Clients
                .Where(c => c.Pesel.Equals(clientTripAssignDTO.Pesel))
                .Count() == 0)
            {
                context.Add(new Client
                {
                    FirstName = clientTripAssignDTO.FirstName,
                    LastName = clientTripAssignDTO.LastName,
                    Email = clientTripAssignDTO.Email,
                    Telephone = clientTripAssignDTO.Telephone,
                    Pesel = clientTripAssignDTO.Pesel
                });
                context.SaveChanges();
            }
            // jeśli klient istnieje, czy nie jest już przypisany do danej wycieczki
            else
            {
                Client client = context.Clients.Where(c => c.Pesel.Equals(clientTripAssignDTO.Pesel))
                                                .Select(c => new Client
                                                {
                                                    FirstName = c.FirstName,
                                                    LastName = c.LastName,
                                                    Email = c.Email,
                                                    Telephone = c.Telephone,
                                                    Pesel = c.Pesel,
                                                    ClientTrips = c.ClientTrips
                                                })
                                                .Single();
                if (client.ClientTrips
                    .Where(ct => ct.IdTrip.Equals(clientTripAssignDTO.IdTrip))
                    .Count() != 0)
                {
                    return BadRequest("Client arleady assigned to this trip!");
                }
            }

            DateTime paymentDate;
            if (!clientTripAssignDTO.PaymentDate.Equals(""))
            {
                context.Add(new ClientTrip
                {
                    IdClient = context.Clients.Where(c => c.Pesel.Equals(clientTripAssignDTO.Pesel)).Select(c => c.IdClient).Single(),
                    IdTrip = idTrip,
                    RegisteredAt = DateTime.Now,
                    PaymentDate = DateTime.Parse(clientTripAssignDTO.PaymentDate)
                });
            }
            else
            {
                context.Add(new ClientTrip
                {
                    IdClient = context.Clients.Where(c => c.Pesel.Equals(clientTripAssignDTO.Pesel)).Select(c => c.IdClient).Single(),
                    IdTrip = idTrip,
                    RegisteredAt = DateTime.Now
                });
            }
            
            context.SaveChanges();

            return Ok("Client has been assigned to trip");

        }

    }
}
