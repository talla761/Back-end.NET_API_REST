using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P7CreateRestApi.DTOs
{
    public class BidListDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incrémentation de l'ID
        public int BidListId { get; set; }

        [Required(ErrorMessage = "Account est obligatoire")]
        [MaxLength(50)]
        public string Account { get; set; }

        [Required(ErrorMessage = "BidType est obligatoire")]
        [MaxLength(50)]
        public string BidType { get; set; }

        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? Bid { get; set; }
        public double? Ask { get; set; }

        [Required(ErrorMessage = "Benchmark est obligatoire")]
        [MaxLength(50)]
        public string Benchmark { get; set; }

        public DateTime? BidListDate { get; set; }

        [Required(ErrorMessage = "Commentary est obligatoire")]
        [MaxLength(255)]
        public string Commentary { get; set; }

        [Required(ErrorMessage = "BidSecurity est obligatoire")]
        [MaxLength(50)]
        public string BidSecurity { get; set; }

        [Required(ErrorMessage = "BidStatus est obligatoire")]
        [MaxLength(20)]
        public string BidStatus { get; set; }

        [Required(ErrorMessage = "Trader est obligatoire")]
        [MaxLength(50)]
        public string Trader { get; set; }

        [Required(ErrorMessage = "Book est obligatoire")]
        [MaxLength(50)]
        public string Book { get; set; }

        [Required(ErrorMessage = "CreationName est obligatoire")]
        [MaxLength(100)]
        public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }

        [Required(ErrorMessage = "RevisionName est obligatoire")]
        [MaxLength(100)]
        public string RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }

        [Required(ErrorMessage = "DealName est obligatoire")]
        [MaxLength(100)]
        public string DealName { get; set; }

        [Required(ErrorMessage = "DealType est obligatoire")]
        [MaxLength(50)]
        public string DealType { get; set; }

        [Required(ErrorMessage = "SourceListId est obligatoire")]
        [MaxLength(50)]
        public string SourceListId { get; set; }

        [Required(ErrorMessage = "Side est obligatoire")]
        [MaxLength(10)]
        public string Side { get; set; }
    }
}
