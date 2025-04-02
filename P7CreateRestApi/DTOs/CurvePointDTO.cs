using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P7CreateRestApi.DTOs
{
    public class CurvePointDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "CurveId est obligatoire")]
        public byte? CurveId { get; set; }
        [Required(ErrorMessage = "AsOfDate est obligatoire")]
        public DateTime? AsOfDate { get; set; }
        [Required(ErrorMessage = "Term est obligatoire")]
        public double? Term { get; set; }
        public double? CurvePointValue { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
