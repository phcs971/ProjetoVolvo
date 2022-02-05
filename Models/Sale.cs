using System.ComponentModel.DataAnnotations;


namespace ProjetoVolvo.Models {
    public class Sale {
        [Key]
        public int Id { get; set; }

        public double Value { get; set; }

        public DateTime Date { get; set; }

        public int Installments { get; set; } = 1;

        public Client Client { get; set; } = null!;

        public string CarId { get; set; } = null!;
        public Car Car { get; set; } = null!;

        public Seller Seller { get; set; } = null!;
    }

    public class SaleInfo {
        public double Value { get; set; }

        public DateTime Date { get; set; }

        public int Installments { get; set; } = 1;
    }
}