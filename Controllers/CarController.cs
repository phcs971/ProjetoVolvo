using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;
using ProjetoVolvo.Singletons;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : Controller {
        [HttpGet]
        public IActionResult Get() {
            try {
                using (var context = new DealershipContext()) {
                    return Ok(context.Cars.Include(c => c.Accessories).ToList());
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{numChassi}")]
        public IActionResult Get(string numChassi) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}) - Car not found");
                        return NotFound("Car not found");
                    }
                    return Ok(car);
                }

            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("system/{version}")]
        public IActionResult GetSystem(int version) {
            try {
                using (var context = new DealershipContext()) {
                    var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Version == version).ToList();
                    if (cars.Count() == 0) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Get(system/{version}) - Car not found");
                        return NotFound("Car not found");
                    }
                    return Ok(cars);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get(system/{version}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("mileage/{mileage}")]
        public IActionResult GetMileage(double mileage) {
            try {
                using (var context = new DealershipContext()) {
                    var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Mileage == mileage).ToList();
                    return Ok(cars);
                }

            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get(mileage/{mileage}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("mileage/greather-than/{mileage}")]
        public IActionResult GetMileageGreaterThan(double mileage) {
            try {
                using (var context = new DealershipContext()) {
                    var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Mileage >= mileage).ToList();
                    return Ok(cars);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get(mileage/greather-than/{mileage}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("mileage/less-than/{mileage}")]
        public IActionResult GetMileageLessThan(double mileage) {
            try {
                using (var context = new DealershipContext()) {
                    var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Mileage < mileage).ToList();
                    return Ok(cars);
                }

            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get(mileage/less-than/{mileage}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("order-by/mileage")]
        public IActionResult GetOrderByMileage() {
            try {
                using (var context = new DealershipContext()) {
                    var cars = context.Cars.Include(c => c.Accessories).OrderByDescending(c => c.Mileage).ToList();
                    return Ok(cars);
                }

            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get(order-by/mileage) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] Car car) {
            try {
                using (var context = new DealershipContext()) {
                    context.Cars.Add(car);
                    context.SaveChanges();
                    return Ok(car);
                }

            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Post - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPut("{numChassi}")]
        public IActionResult Put(string numChassi, [FromBody] Car car) {
            try {
                using (var context = new DealershipContext()) {
                    var carToUpdate = context.Cars.Find(numChassi);
                    if (carToUpdate == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Put({numChassi}) - Car not found");
                        return NotFound("Car not found");
                    }
                    carToUpdate.Model = car.Model;
                    carToUpdate.Mileage = car.Mileage;
                    carToUpdate.Version = car.Version;
                    carToUpdate.Year = car.Year;
                    carToUpdate.Color = car.Color;
                    context.SaveChanges();
                    return Get(numChassi);
                }

            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Put({numChassi}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpDelete("{numChassi}")]
        public IActionResult Delete(string numChassi) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Sale).Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Delete({numChassi}) - Car not found");
                        return NotFound("Car not found");
                    }
                    if (car.Sale != null) context.Sales.Remove(car.Sale);
                    context.Cars.Remove(car);
                    context.SaveChanges();
                    return Ok(null);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Delete({numChassi}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{numChassi}/accessories")]
        public IActionResult GetAccessories(string numChassi) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/accessories) - Car not found");
                        return NotFound("Car not found");
                    }
                    return Ok(car.Accessories);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/accessories) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }


        [HttpPost("{numChassi}/accessories")]
        public IActionResult PostAccessory(string numChassi, [FromBody] Accessory accessory) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Post({numChassi}/accessories) - Car not Found");
                        return NotFound("Car not Found");
                    }
                    car.Accessories.Add(accessory);
                    context.SaveChanges();
                    return Ok(accessory);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Post({numChassi}/accessories) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }


        [HttpPost("{numChassi}/accessories/{accessoryId}")]
        public IActionResult PostAccessory(string numChassi, int accessoryId) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Post({numChassi}/accessories/{accessoryId}) - Car not Found");
                        return NotFound("Car not Found");
                    }
                    if (car.Accessories.Any(a => a.AccessoryId == accessoryId)) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Post({numChassi}/accessories/{accessoryId}) - This car already have this accessory");
                        return BadRequest("This car already have this accessory");
                        }
                    var accessory = context.Accessories.Find(accessoryId);
                    if (accessory == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Post({numChassi}/accessories/{accessoryId}) - Accessory not Found");
                        return NotFound("Accessory not Found");
                    }
                    car.Accessories.Add(accessory);
                    context.SaveChanges();
                    return Ok(car);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Post({numChassi}/accessories/{accessoryId}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpDelete("{numChassi}/accessories/{id}")]
        public IActionResult DeleteAccessory(string numChassi, int id) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Delete({numChassi}/accessories/{id}) - Car not found");
                        return NotFound("Car not found");
                    }
                    var accessory = car.Accessories.Where(a => a.AccessoryId == id).FirstOrDefault();
                    if (accessory == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Delete({numChassi}/accessories/{id}) - Accessory not found");
                        return NotFound("Accessory not found");
                    }
                    car.Accessories.Remove(accessory);
                    context.SaveChanges();
                    return Ok(null);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Delete({numChassi}/accessories/{id}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{numChassi}/sale")]
        public IActionResult GetSale(string numChassi) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Sale).ThenInclude(s => s!.Client).Include(c => c.Sale).ThenInclude(s => s!.Seller).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/sale) - Car not found");
                        return NotFound("Car not found");
                    }
                    if (car.Sale == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/sale) - Car was not sold");
                        return NotFound("Car was not sold");
                    }
                    return Ok(car.Sale);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/sale) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{numChassi}/owner")]
        public IActionResult GetOwner(string numChassi) {
            try {
                using (var context = new DealershipContext()) {
                    var car = context.Cars.Include(c => c.Sale).ThenInclude(s => s!.Client).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                    if (car == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/owner) - Car not found");
                        return NotFound("Car not found");
                    }
                    if (car.Sale == null) {
                        LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/owner) - Car was not sold");
                        return NotFound("Car was not sold");
                    }
                    return Ok(car.Sale.Client);
                }
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<CarController> Get({numChassi}/owner) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }
    }
}