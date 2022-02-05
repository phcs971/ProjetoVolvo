using System.ComponentModel.DataAnnotations;

namespace ProjetoVolvo.Models {
    public class Car {
        [Key]
        [MaxLength(20)]
        public string NumChassi { get; set; } = null!;

        [MaxLength(30)]
        public string Model { get; set; } = null!;
        
        public int Year { get; set; }

        [MaxLength(30)]
        public string? Color { get; set; }
        
        public double Mileage { get; set; } = 0;
        
        public int Version { get; set; }

        public ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();

        public Sale? Sale { get; set; }
    }
}