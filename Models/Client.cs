using System.ComponentModel.DataAnnotations;


namespace ProjetoVolvo.Models
{
    public class Client
    {
        [Key]
        [StringLength(14)]
        public string Cpf_cnpj { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(25)]
        public string? Email { get; set; }

        [MaxLength(14)]
        public string? Phone { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        
        public Address? Address { get; set; }

    }
}