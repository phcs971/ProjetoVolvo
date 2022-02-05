using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProjetoVolvo.Models {
    public class Accessory {
        [Key]
        public int AccessoryId { get; set; }

        [MaxLength(50)]
        public string Description { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}