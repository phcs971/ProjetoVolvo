using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoVolvo.Singletons;
using com.valgut.libs.bots.Wit;
using com.valgut.libs.bots.Wit.Models;

namespace ProjetoVolvo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : Controller {
        private readonly CarController CarController;
        private readonly SellerController SellerController;

        public AIController(CarController carController, SellerController sellerController) {
            CarController = carController;
            SellerController = sellerController;
        }


        [HttpGet("{message}")]
        public IActionResult Get(string message) {
            try {
                var client = new WitClient("UAZDPY4FRWFBGTSZ4OW6I5H4HBT6PSQZ");
                Message m = client.GetMessage(message);
                var intents = m.entities["intent"];
                var intent = intents?.FirstOrDefault();
                if (intents == null || intent == null) {
                    LogSingleton.Instance.ErrorLog($"<AIController> Get({message}) - No Intent Found!");
                    return BadRequest("Não entendi");
                }
                switch (intent.value.ToString()) {
                    case "get_cars_by_mileage":
                        return CarController.GetOrderByMileage();
                    case "get_cars_by_version":
                        var number = m.entities["number"]?.FirstOrDefault();
                        if (number == null) {
                            LogSingleton.Instance.ErrorLog($"<AIController> Get({message}) - No Version Number Found!");
                            return BadRequest("Não entendi o número da versão");
                        }
                        return CarController.GetSystem(((int)number.value));
                    case "get_wage_by_id":
                        DateTime date;
                        try {
                            date = ((DateTime?) m.entities["datetime"]?.FirstOrDefault()?.value) ?? DateTime.Now;
                        } catch (Exception) {
                            date = DateTime.Now;
                        }
                        var cpf = ((string?) m.entities["cpf"]?.FirstOrDefault()?.value);
                        if (cpf == null) {
                            LogSingleton.Instance.ErrorLog($"<AIController> Get({message}) - No Seller Id Found!");
                            return BadRequest("Não entendi o cpf do vendedor");
                        }
                        return SellerController.GetWage(cpf, date.Month, date.Year);
                }
                LogSingleton.Instance.ErrorLog($"<AIController> Get({message}) - Strange Intent Found: {intent.value.ToString()}");
                return BadRequest("Não entendi");
            } catch (Exception ex) {
                LogSingleton.Instance.ErrorLog($"<AIController> Get({message}) - {ex.Message}");
                return StatusCode(500, $"Internal Server Error\n{ex.Message}");
            }
        }
    }
}