using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;


namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoryController : Controller {

        [HttpGet]
        public IActionResult Get() {
            using var context = new DealershipContext();
            return Ok(context.Accessories.ToList());
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            using var context = new DealershipContext();
            var accessory = context.Accessories.Find(id);
            if (accessory == null) return NotFound();
            return Ok(accessory);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] Accessory accessory) {
            using var context = new DealershipContext();
            context.Accessories.Add(accessory);
            context.SaveChanges();
            return Ok(accessory);
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Accessory accessory) {
            using var context = new DealershipContext();
            var accessoryToUpdate = context.Accessories.Find(id);
            if (accessoryToUpdate == null) return NotFound();
            accessoryToUpdate.Description = accessory.Description;
            context.SaveChanges();
            return Ok(accessoryToUpdate);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            using var context = new DealershipContext();
            var accessoryToDelete = context.Accessories.Find(id);
            if (accessoryToDelete == null) return NotFound();
            context.Accessories.Remove(accessoryToDelete);
            context.SaveChanges();
            return Ok(null);
        }
    }
}