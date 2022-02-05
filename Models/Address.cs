using System.ComponentModel.DataAnnotations;


namespace ProjetoVolvo.Models {
    public class Address {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        public string Cep { get; set; } = null!;

        [MaxLength(50)]
        public string Street { get; set; } = null!;

        [MaxLength(10)]
        public string Number { get; set; } = null!;

        [MaxLength(25)]
        public string? Complement { get; set; }

        [MaxLength(20)]
        public string Neighborhood { get; set; } = null!;

        [MaxLength(20)]
        public string City { get; set; } = null!;

        [MaxLength(2)]
        public string State { get; set; } = null!;
    }
}