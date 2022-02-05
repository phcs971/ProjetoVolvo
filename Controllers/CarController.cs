using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : Controller {
        [HttpGet]
        public IActionResult Get() {
            using (var context = new DealershipContext()) {
                return Ok(context.Cars.Include(c => c.Accessories).ToList());
            }
        }

        [HttpGet("{numChassi}")]
        public IActionResult Get(string numChassi) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound();
                return Ok(car);
            }
        }

        [HttpGet("system/{version}")]
        public IActionResult GetSystem(int version) {
            using (var context = new DealershipContext()) {
                var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Version == version).ToList();
                if (cars.Count() == 0) return NotFound();
                return Ok(cars);
            }
        }

        [HttpGet("mileage/{mileage}")]
        public IActionResult GetMileage(double mileage) {
            using (var context = new DealershipContext()) {
                var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Mileage == mileage).ToList();
                return Ok(cars);
            }
        }

        [HttpGet("mileage/greather-than/{mileage}")]
        public IActionResult GetMileageGreaterThan(double mileage) {
            using (var context = new DealershipContext()) {
                var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Mileage >= mileage).ToList();
                return Ok(cars);
            }
        }

        [HttpGet("mileage/less-than/{mileage}")]
        public IActionResult GetMileageLessThan(double mileage) {
            using (var context = new DealershipContext()) {
                var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Mileage < mileage).ToList();
                return Ok(cars);
            }
        }

        [HttpGet("order-by/mileage")]
        public IActionResult GetOrderByMileage() {
            using (var context = new DealershipContext()) {
                var cars = context.Cars.Include(c => c.Accessories).OrderBy(c => c.Mileage).ToList();
                return Ok(cars);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] Car car) {
            using (var context = new DealershipContext()) {
                context.Cars.Add(car);
                context.SaveChanges();
                return Ok(car);
            }
        }

        [HttpPut("{numChassi}")]
        public IActionResult Put(string numChassi, [FromBody] Car car) {
            using (var context = new DealershipContext()) {
                var carToUpdate = context.Cars.Find(numChassi);
                if (carToUpdate == null) return NotFound();
                carToUpdate.Model = car.Model;
                carToUpdate.Mileage = car.Mileage;
                carToUpdate.Version = car.Version;
                carToUpdate.Year = car.Year;
                carToUpdate.Color = car.Color;
                context.SaveChanges();
                return Get(numChassi);
            }
        }

        [HttpDelete("{numChassi}")]
        public IActionResult Delete(string numChassi) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Sale).Include(c=> c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound();
                if (car.Sale != null) context.Sales.Remove(car.Sale);
                context.Cars.Remove(car);
                context.SaveChanges();
                return Ok(null);
            }
        }

        [HttpGet("{numChassi}/accessories")]
        public IActionResult GetAccessories(string numChassi) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound();
                return Ok(car.Accessories);
            }
        }


        [HttpPost("{numChassi}/accessories")]
        public IActionResult PostAccessory(string numChassi, [FromBody] Accessory accessory) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound("Car not Found");
                car.Accessories.Add(accessory);
                context.SaveChanges();
                return Ok(accessory);
            }
        }

        
        [HttpPost("{numChassi}/accessories/{accessoryId}")]
        public IActionResult PostAccessory(string numChassi, int accessoryId) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound("Car not Found");
                if (car.Accessories.Any(a => a.AccessoryId == accessoryId)) return BadRequest("Accessory already exists");
                var accessory = context.Accessories.Find(accessoryId);
                if (accessory == null) return NotFound("Accessory not Found");
                car.Accessories.Add(accessory);
                context.SaveChanges();
                return Ok(car);
            }
        }

        [HttpDelete("{numChassi}/accessories/{id}")]
        public IActionResult DeleteAccessory(string numChassi, int id) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Accessories).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound("Car not found");
                var accessory = car.Accessories.Where(a => a.AccessoryId == id).FirstOrDefault();
                if (accessory == null) return NotFound("Accessory not found");
                car.Accessories.Remove(accessory);
                context.SaveChanges();
                return Ok(null);
            }
        }

        [HttpGet("{numChassi}/sale")]
        public IActionResult GetSale(string numChassi) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Sale).ThenInclude(s => s!.Client).Include(c => c.Sale).ThenInclude(s => s!.Seller).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound("Car not found");
                if (car.Sale == null) return NotFound("Car was not sold");
                return Ok(car.Sale);
            }
        }

        [HttpGet("{numChassi}/owner")]
        public IActionResult GetOwner(string numChassi) {
            using (var context = new DealershipContext()) {
                var car = context.Cars.Include(c => c.Sale).ThenInclude(s => s!.Client).Where(c => c.NumChassi == numChassi).FirstOrDefault();
                if (car == null) return NotFound("Car not found");
                if (car.Sale == null) return NotFound("Car was not sold");
                return Ok(car.Sale.Client);
            }
        }
    }
}