using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVolvo.Models;
using ProjetoVolvo.Singletons;
using ProjetoVolvo.Controllers;

namespace ProjetoVolvo.Repository {
    public class AIRepository {
        // public IActionResult CarGetSystem(int version) {
            // try {
            //     using (var context = new DealershipContext()) {
            //         var cars = context.Cars.Include(c => c.Accessories).Where(c => c.Version == version).ToList();
            //         if (cars.Count() == 0) {
            //             LogSingleton.Instance.ErrorLog($"<CarController> Get(system/{version}) - Car not found");
            //             return ControllerB().NotFound("Car not found");
            //         }
            //         return Ok(cars);
            //     }
            // } catch (Exception ex) {
            //     LogSingleton.Instance.ErrorLog($"<CarController> Get(system/{version}) - {ex.Message}");
            //     return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            // }
        // }
    }
}