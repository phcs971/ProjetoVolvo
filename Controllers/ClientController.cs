using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : Controller {
        [HttpGet]
        public IActionResult Get() {
            using var context = new DealershipContext();
            return Ok(context.Clients.Include(c => c.Address).ToList());
        }

        [HttpGet("{cpf_cnpj}")]
        public IActionResult Get(string cpf_cnpj) {
            using var context = new DealershipContext();
            var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client) {
            using var context = new DealershipContext();
            context.Clients.Add(client);
            context.SaveChanges();
            return Ok(client);
        }

        [HttpPut("{cpf_cnpj}")]
        public IActionResult Put(string cpf_cnpj, [FromBody] Client client) {
            using var context = new DealershipContext();
            var clientToUpdate = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (clientToUpdate == null) return NotFound();
            clientToUpdate.Name = client.Name;
            clientToUpdate.Email = client.Email;
            clientToUpdate.Phone = client.Phone;
            context.SaveChanges();
            return Ok(clientToUpdate);
        }

        [HttpDelete("{cpf_cnpj}")]
        public IActionResult Delete(string cpf_cnpj) {
            using var context = new DealershipContext();
            var clientToDelete = context.Clients.Include(c => c.Address).Include(c => c.Sales).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (clientToDelete == null) return NotFound();
            context.Sales.RemoveRange(clientToDelete.Sales);
            context.Clients.Remove(clientToDelete);
            context.SaveChanges();
            return Ok(null);
        }

        [HttpGet("{cpf_cnpj}/address")]
        public IActionResult GetAddress(string cpf_cnpj) {
            using var context = new DealershipContext();
            var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (client == null) return NotFound("Client not found");
            if (client.Address == null) return NotFound("Address not found");
            return Ok(client.Address);
        }

        [HttpPost("{cpf_cnpj}/address")]
        public IActionResult PostAddress(string cpf_cnpj, [FromBody] Address address) {
            using var context = new DealershipContext();
            var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (client == null) return NotFound();
            client.Address = address;
            context.SaveChanges();
            return Ok(client);
        }

        [HttpPut("{cpf_cnpj}/address")]
        public IActionResult PutAddress(string cpf_cnpj, [FromBody] Address address) {
            using var context = new DealershipContext();
            var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (client == null) return NotFound();
            client.Address = address;
            context.SaveChanges();
            return Ok(client);
        }

        [HttpDelete("{cpf_cnpj}/address")]
        public IActionResult DeleteAddress(string cpf_cnpj) {
            using var context = new DealershipContext();
            var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (client == null) return NotFound();
            if (client.Address == null) return BadRequest("Client has no Address");
            context.Addresses.Remove(client.Address);
            client.Address = null;
            context.SaveChanges();
            return Ok(client);
        }

        [HttpGet("{cpf_cnpj}/cars")]
        public IActionResult GetCars(string cpf_cnpj) {
            using var context = new DealershipContext();
            var client = context.Clients.Include(c => c.Sales).ThenInclude(s => s.Car).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (client == null) return NotFound();
            return Ok(client.Sales.Select(s => s.Car));
        }

        [HttpGet("{cpf_cnpj}/transactions")]
        public IActionResult GetTransactions(string cpf_cnpj) {
            using var context = new DealershipContext();
            var client = context.Clients.Include(c => c.Sales).ThenInclude(s => s.Car).Include(c => c.Sales).ThenInclude(s => s.Seller).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
            if (client == null) return NotFound();
            return Ok(client.Sales);
        }
    }
}