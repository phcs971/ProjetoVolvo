using System.ComponentModel.DataAnnotations;


namespace ProjetoVolvo.Models {
    public class Seller {
        [Key]
        [StringLength(11)]
        public string Cpf { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [MaxLength(50)]
        public string Email { get; set; } = null!;

        public double Wage { get; set; }

        public int WorkLoad { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

    }
}