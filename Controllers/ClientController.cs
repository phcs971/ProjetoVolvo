using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;
using ProjetoVolvo.Singletons;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : Controller {
        [HttpGet]
        public IActionResult Get() {
            try {
                using var context = new DealershipContext();
                return Ok(context.Clients.Include(c => c.Address).ToList());
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Get - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf_cnpj}")]
        public IActionResult Get(string cpf_cnpj) {
            try {
                using var context = new DealershipContext();
                var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (client == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}) - Client not found");
                    return NotFound("Client not found");
                }
                return Ok(client);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client) {
            try {
                using var context = new DealershipContext();
                context.Clients.Add(client);
                context.SaveChanges();
                return Ok(client);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Post - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPut("{cpf_cnpj}")]
        public IActionResult Put(string cpf_cnpj, [FromBody] Client client) {
            try {
                using var context = new DealershipContext();
                var clientToUpdate = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (clientToUpdate == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Put({cpf_cnpj}) - Client not found");
                    return NotFound("Client not found");
                }
                clientToUpdate.Name = client.Name;
                clientToUpdate.Email = client.Email;
                clientToUpdate.Phone = client.Phone;
                context.SaveChanges();
                return Ok(clientToUpdate);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Put({cpf_cnpj}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpDelete("{cpf_cnpj}")]
        public IActionResult Delete(string cpf_cnpj) {
            try {
                using var context = new DealershipContext();
                var clientToDelete = context.Clients.Include(c => c.Address).Include(c => c.Sales).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (clientToDelete == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Delete({cpf_cnpj}) - Client not found");
                    return NotFound("Client not found");
                }
                context.Sales.RemoveRange(clientToDelete.Sales);
                context.Clients.Remove(clientToDelete);
                context.SaveChanges();
                return Ok(null);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Delete({cpf_cnpj}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf_cnpj}/address")]
        public IActionResult GetAddress(string cpf_cnpj) {
            try {
                using var context = new DealershipContext();
                var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (client == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}/address) - Client not found");
                    return NotFound("Client not found");
                }
                if (client.Address == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}/address) - Address not found");
                    return NotFound("Address not found");
                }
                return Ok(client.Address);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}/address) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPost("{cpf_cnpj}/address")]
        public IActionResult PostAddress(string cpf_cnpj, [FromBody] Address address) {
            try {
                using var context = new DealershipContext();
                var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (client == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Post({cpf_cnpj}/address) - Client not found");
                    return NotFound("Client not found");
                }
                client.Address = address;
                context.SaveChanges();
                return Ok(client);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Post({cpf_cnpj}/address) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPut("{cpf_cnpj}/address")]
        public IActionResult PutAddress(string cpf_cnpj, [FromBody] Address address) {
            try {
                using var context = new DealershipContext();
                var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (client == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Put({cpf_cnpj}/address) - Client not found");
                    return NotFound("Client not found");
                }
                client.Address = address;
                context.SaveChanges();
                return Ok(client);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Put({cpf_cnpj}/address) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpDelete("{cpf_cnpj}/address")]
        public IActionResult DeleteAddress(string cpf_cnpj) {
            try {
                using var context = new DealershipContext();
                var client = context.Clients.Include(c => c.Address).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (client == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Delete({cpf_cnpj}/address) - Client not found");
                    return NotFound("Client not found");
                }
                if (client.Address == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Delete({cpf_cnpj}/address) - Client has not Address");
                    return BadRequest("Client has no Address");
                }
                context.Addresses.Remove(client.Address);
                client.Address = null;
                context.SaveChanges();
                return Ok(client);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Delete({cpf_cnpj}/address) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf_cnpj}/cars")]
        public IActionResult GetCars(string cpf_cnpj) {
            try {
                using var context = new DealershipContext();
                var client = context.Clients.Include(c => c.Sales).ThenInclude(s => s.Car).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (client == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}/cars) - Client not found");
                    return NotFound("Client not found");
                }
                return Ok(client.Sales.Select(s => s.Car));
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}/cars) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf_cnpj}/transactions")]
        public IActionResult GetTransactions(string cpf_cnpj) {
            try {
                using var context = new DealershipContext();
                var client = context.Clients.Include(c => c.Sales).ThenInclude(s => s.Car).Include(c => c.Sales).ThenInclude(s => s.Seller).Where(c => c.Cpf_cnpj == cpf_cnpj).FirstOrDefault();
                if (client == null) {
                    LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}/transactions) - Client not found");
                    return NotFound("Client not found");
                }
                return Ok(client.Sales);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<ClientController> Get({cpf_cnpj}/transactions) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }
    }
}