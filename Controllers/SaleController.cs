using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : Controller {
        [HttpGet]
        public IActionResult Get() {
            using (var context = new DealershipContext()) {
                return Ok(context.Sales.Include(s => s.Car).Include(s => s.Client).Include(s => s.Seller).ToList());
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            using (var context = new DealershipContext()) {
                var sale = context.Sales.Include(s => s.Car).Include(s => s.Client).Include(s => s.Seller).Where(s => s.Id == id).FirstOrDefault();
                if (sale == null) return NotFound();
                return Ok(sale);
            }
        }

        [HttpPost("seller/{sellerCpf}/car/{carNumChassi}/client/{clientCpfCnpj}")]
        public IActionResult Post( string clientCpfCnpj,  string carNumChassi,  string sellerCpf, [FromBody] SaleInfo saleInfo) {
            using (var context = new DealershipContext()) {
                var client = context.Clients.Where(c => c.Cpf_cnpj == clientCpfCnpj).FirstOrDefault();
                if (client == null) return NotFound("Client not found");
                var car = context.Cars.Where(c => c.NumChassi == carNumChassi).FirstOrDefault();
                if (car == null) return NotFound("Car not found");
                var seller = context.Sellers.Where(s => s.Cpf == sellerCpf).FirstOrDefault();
                if (seller == null) return NotFound("Seller not found");
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
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SaleInfo sale) {
            using (var context = new DealershipContext()) {
                var saleToUpdate = context.Sales.Where(s => s.Id == id).FirstOrDefault();
                if (saleToUpdate == null) return NotFound();
                saleToUpdate.Value = sale.Value;
                saleToUpdate.Date = sale.Date;
                saleToUpdate.Installments = sale.Installments;
                context.SaveChanges();
                return Get(id);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            using (var context = new DealershipContext()) {
                var saleToDelete = context.Sales.Where(s => s.Id == id).FirstOrDefault();
                if (saleToDelete == null) return NotFound();
                context.Sales.Remove(saleToDelete);
                context.SaveChanges();
                return Ok(null);
            }
        }
    }
}