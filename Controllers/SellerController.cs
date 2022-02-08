using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;
using ProjetoVolvo.Singletons;


namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            try {
                using (var context = new DealershipContext()) {
                    return Ok(context.Sellers.ToList());
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Get - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf}")]
        public IActionResult Get(string cpf) {
            try {
                using (var context = new DealershipContext()) {
                    var seller = context.Sellers.Where(s => s.Cpf == cpf).FirstOrDefault();
                    if (seller == null) {
                        LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}) - Seller not found");
                        return NotFound("Seller not found");
                    }
                    return Ok(seller);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Seller seller) {
            try {
                using (var context = new DealershipContext()) {
                    context.Sellers.Add(seller);
                    context.SaveChanges();
                    return Ok(seller);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Post - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPut("{cpf}")]
        public IActionResult Put(string cpf, [FromBody] Seller seller) {
            try {
                using (var context = new DealershipContext()) {
                    var sellerToUpdate = context.Sellers.Where(s => s.Cpf == cpf).FirstOrDefault();
                    if (sellerToUpdate == null) {
                        LogSingleton.Instance.ErrorLog($"<SellerController> Put({cpf}) - Seller not found");
                        return NotFound("Seller not found");
                    }
                    sellerToUpdate.Name = seller.Name;
                    sellerToUpdate.Phone = seller.Phone;
                    sellerToUpdate.Email = seller.Email;
                    sellerToUpdate.Wage = seller.Wage;
                    sellerToUpdate.WorkLoad = seller.WorkLoad;
                    context.SaveChanges();
                    return Ok(sellerToUpdate);
                }

            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Put({cpf}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpDelete("{cpf}")]
        public IActionResult Delete(string cpf) {
            try {
                using (var context = new DealershipContext()) {
                    var sellerToDelete = context.Sellers.Where(s => s.Cpf == cpf).FirstOrDefault();
                    if (sellerToDelete == null) {
                        LogSingleton.Instance.ErrorLog($"<SellerController> Delete({cpf}) - Seller not found");
                        return NotFound("Seller not found");
                    }
                    context.Sellers.Remove(sellerToDelete);
                    context.Sales.RemoveRange(sellerToDelete.Sales);
                    context.SaveChanges();
                    return Ok(null);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Delete({cpf}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf}/sales")]
        public IActionResult GetSales(string cpf) {
            try {
                using (var context = new DealershipContext()) {
                    var seller = context.Sellers.Include(s => s.Sales).ThenInclude(s => s.Car).Include(c => c.Sales).ThenInclude(s => s.Client).Where(s => s.Cpf == cpf).FirstOrDefault();
                    if (seller == null) {
                        LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}/sales) - Seller not found");
                        return NotFound("Seller not found");
                    }
                    return Ok(seller.Sales);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}/sales) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf}/sales/{month}/{year}")]
        public IActionResult GetSales(string cpf, int month, int year) {
            try {
                using (var context = new DealershipContext()) {
                    var seller = context.Sellers.Include(s => s.Sales).ThenInclude(s => s.Car).Include(c => c.Sales).ThenInclude(s => s.Client).Where(s => s.Cpf == cpf).FirstOrDefault();
                    if (seller == null) {
                        LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}/sales/{month}/{year}) - Seller not found");
                        return NotFound("Seller not found");
                    }
                    return Ok(seller.Sales.Where(s => s.Date.Month == month && s.Date.Year == year).ToList());
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}/sales/{month}/{year}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{cpf}/wage/{month}/{year}")]
        public IActionResult GetWage(string cpf, int month, int year) {
            try {
                using (var context = new DealershipContext()) {
                    var seller = context.Sellers.Include(s => s.Sales).Where(s => s.Cpf == cpf).FirstOrDefault();
                    if (seller == null) {
                        LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}/wage/{month}/{year}) - Seller not found");
                        return NotFound("Seller not found");
                    }
                    var sales = seller.Sales.Where(s => s.Date.Month == month && s.Date.Year == year).Sum(s => s.Value);
                    var wage = seller.Wage + sales / 100;
                    return Ok(wage);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<SellerController> Get({cpf}/wage/{month}/{year}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }
    }
}