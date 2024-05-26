using Cwiczenia_5.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia_5.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        [HttpGet]
        public IActionResult getClients()
        {
            return Ok(new s16467Context().Clients);
        }

        [HttpDelete("{idClient}")]
        public IActionResult deleteClient(int idClient)
        {
            var context = new s16467Context();
            Client client = context.Clients
                .Where(c => c.IdClient.Equals(idClient))
                .Select(c => new Client()
                {
                    IdClient = c.IdClient,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Telephone = c.Telephone,
                    Pesel = c.Pesel,
                    ClientTrips = c.ClientTrips

                }).SingleOrDefault();
            

            if (client == null)
                return NotFound("Client with this id does not exist");

            if (client.ClientTrips.Any())
                return BadRequest("Client has assigned trips!");

            context.Remove(client);

            context.SaveChanges();

            return Ok("Client has been deleted");
        }
    }
}
