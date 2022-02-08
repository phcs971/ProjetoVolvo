using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;
using ProjetoVolvo.Singletons;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : Controller {
        [HttpGet]
        public IActionResult Get() {
            try {
                using (var context = new DealershipContext()) {
                    return Ok(context.Sales.Include(s => s.Car).Include(s => s.Client).Include(s => s.Seller).ToList());
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SaleController> Get - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            try {
                using (var context = new DealershipContext()) {
                    var sale = context.Sales.Include(s => s.Car).Include(s => s.Client).Include(s => s.Seller).Where(s => s.Id == id).FirstOrDefault();
                    if (sale == null) {
                        LogSingleton.Instance.ErrorLog($"<SaleController> Get({id}) - Sale not found");
                        return NotFound("Sale not found");
                    }
                    return Ok(sale);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SaleController> Get({id}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPost("seller/{sellerCpf}/car/{carNumChassi}/client/{clientCpfCnpj}")]
        public IActionResult Post(string clientCpfCnpj, string carNumChassi, string sellerCpf, [FromBody] SaleInfo saleInfo) {
            try {
                using (var context = new DealershipContext()) {
                    var client = context.Clients.Where(c => c.Cpf_cnpj == clientCpfCnpj).FirstOrDefault();
                    if (client == null) {
                        LogSingleton.Instance.ErrorLog($"<SaleController> Post(seller/{sellerCpf}/car/{carNumChassi}/client/{clientCpfCnpj}) - Client not found");
                        return NotFound("Client not found");
                    }
                    var car = context.Cars.Where(c => c.NumChassi == carNumChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<SaleController> Post(seller/{sellerCpf}/car/{carNumChassi}/client/{clientCpfCnpj}) - Car not found");
                        return NotFound("Car not found");
                    }
                    var seller = context.Sellers.Where(s => s.Cpf == sellerCpf).FirstOrDefault();
                    if (seller == null) {
                        LogSingleton.Instance.ErrorLog($"<SaleController> Post(seller/{sellerCpf}/car/{carNumChassi}/client/{clientCpfCnpj}) - Seller not found");
                        return NotFound("Seller not found");
                    }
                    var sale = new Sale {
                        Client = client,
                        Car = car,
                        Seller = seller,
                        Date = saleInfo.Date,
                        Value = saleInfo.Value,
                        Installments = saleInfo.Installments,
                    };

                    context.Sales.Add(sale);
                    context.SaveChanges();
                    return Ok(sale);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SaleController> Post(seller/{sellerCpf}/car/{carNumChassi}/client/{clientCpfCnpj}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SaleInfo sale) {
            try {
                using (var context = new DealershipContext()) {
                    var saleToUpdate = context.Sales.Where(s => s.Id == id).FirstOrDefault();
                    if (saleToUpdate == null) {
                        LogSingleton.Instance.ErrorLog($"<SaleController> Put({id}) - Sale not found");
                        return NotFound("Sale not found");
                    }
                    saleToUpdate.Value = sale.Value;
                    saleToUpdate.Date = sale.Date;
                    saleToUpdate.Installments = sale.Installments;
                    context.SaveChanges();
                    return Get(id);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SaleController> Put({id}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            try {
                using (var context = new DealershipContext()) {
                    var saleToDelete = context.Sales.Where(s => s.Id == id).FirstOrDefault();
                    if (saleToDelete == null) {
                        LogSingleton.Instance.ErrorLog($"<SaleController> Delete({id}) - Sale not found");
                        return NotFound("Sale not found");
                    }
                    context.Sales.Remove(saleToDelete);
                    context.SaveChanges();
                    return Ok(null);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SaleController> Delete({id}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }
    }
}