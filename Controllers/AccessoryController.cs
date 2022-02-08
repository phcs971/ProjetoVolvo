using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;
using ProjetoVolvo.Singletons;



namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AccessoryController : Controller {

        [HttpGet]
        public IActionResult Get() {
            try {
                using var context = new DealershipContext();
                return Ok(context.Accessories.ToList());
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<AccessoryController> Get - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            try {
                using var context = new DealershipContext();
                var accessory = context.Accessories.Find(id);
                if (accessory == null) {
                    LogSingleton.Instance.ErrorLog($"<AccessoryController> Get({id}) - Accessory not found");
                    return NotFound("Accessory not found");
                }
                return Ok(accessory);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<AccessoryController> Get({id}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Accessory accessory) {
            try {
                using var context = new DealershipContext();
                context.Accessories.Add(accessory);
                context.SaveChanges();
                return Ok(accessory);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<AccessoryController> Post - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Accessory accessory) {
            try {
                using var context = new DealershipContext();
                var accessoryToUpdate = context.Accessories.Find(id);
                if (accessoryToUpdate == null) {
                    LogSingleton.Instance.ErrorLog($"<AccessoryController> Put({id}) - Accessory not found");
                    return NotFound("Accessory not found");
                }
                accessoryToUpdate.Description = accessory.Description;
                context.SaveChanges();
                return Ok(accessoryToUpdate);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<AccessoryController> Put({id}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            try {
                using var context = new DealershipContext();
                var accessoryToDelete = context.Accessories.Find(id);
                if (accessoryToDelete == null) {
                    LogSingleton.Instance.ErrorLog($"<AccessoryController> Delete({id}) - Accessory not found");
                    return NotFound("Accessory not found");
                }
                context.Accessories.Remove(accessoryToDelete);
                context.SaveChanges();
                return Ok(null);
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<AccessoryController> Delete({id}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }
    }
}