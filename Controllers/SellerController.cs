using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            using (var context = new DealershipContext()) {
                return Ok(context.Sellers.ToList());
            }
        }

        [HttpGet("{cpf}")]
        public IActionResult Get(string cpf) {
            using (var context = new DealershipContext()) {
                var seller = context.Sellers.Where(s => s.Cpf == cpf).FirstOrDefault();
                if (seller == null) return NotFound();
                return Ok(seller);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Seller seller) {
            using (var context = new DealershipContext()) {
                context.Sellers.Add(seller);
                context.SaveChanges();
                return Ok(seller);
            }
        }

        [HttpPut("{cpf}")]
        public IActionResult Put(string cpf, [FromBody] Seller seller) {
            using (var context = new DealershipContext()) {
                var sellerToUpdate = context.Sellers.Where(s => s.Cpf == cpf).FirstOrDefault();
                if (sellerToUpdate == null) return NotFound();
                sellerToUpdate.Name = seller.Name;
                sellerToUpdate.Phone = seller.Phone;
                sellerToUpdate.Email = seller.Email;
                sellerToUpdate.Wage = seller.Wage;
                sellerToUpdate.WorkLoad = seller.WorkLoad;
                context.SaveChanges();
                return Ok(sellerToUpdate);
            }
        }

        [HttpDelete("{cpf}")]
        public IActionResult Delete(string cpf) {
            using (var context = new DealershipContext()) {
                var sellerToDelete = context.Sellers.Where(s => s.Cpf == cpf).FirstOrDefault();
                if (sellerToDelete == null) return NotFound();
                context.Sellers.Remove(sellerToDelete);
                context.Sales.RemoveRange(sellerToDelete.Sales);
                context.SaveChanges();
                return Ok(null);
            }
        }

        [HttpGet("{cpf}/sales")]
        public IActionResult GetSales(string cpf) {
            using (var context = new DealershipContext()) {
                var seller = context.Sellers.Include(s => s.Sales).ThenInclude(s => s.Car).Include(c => c.Sales).ThenInclude(s => s.Client).Where(s => s.Cpf == cpf).FirstOrDefault();
                if (seller == null) return NotFound();
                return Ok(seller.Sales);
            }
        }

        [HttpGet("{cpf}/sales/{month}/{year}")]
        public IActionResult GetSales(string cpf, int month, int year) {
            using (var context = new DealershipContext()) {
                var seller = context.Sellers.Include(s => s.Sales).ThenInclude(s => s.Car).Include(c => c.Sales).ThenInclude(s => s.Client).Where(s => s.Cpf == cpf).FirstOrDefault();
                if (seller == null) return NotFound();
                return Ok(seller.Sales.Where(s => s.Date.Month == month && s.Date.Year == year).ToList());
            }
        }

        [HttpGet("{cpf}/wage/{month}/{year}")]
        public IActionResult GetWage(string cpf, int month, int year) {
            using (var context = new DealershipContext()) {
                var seller = context.Sellers.Include(s => s.Sales).Where(s => s.Cpf == cpf).FirstOrDefault();
                if (seller == null) return NotFound();
                var sales = seller.Sales.Where(s => s.Date.Month == month && s.Date.Year == year).Sum(s => s.Value);
                var wage = seller.Wage + sales / 100;
                return Ok(wage);
            }
        }
    }
}