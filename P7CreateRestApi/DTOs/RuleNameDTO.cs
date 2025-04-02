using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P7CreateRestApi.DTOs
{
    public class RuleNameDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name est obligatoire")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description est obligatoire")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Json est obligatoire")]
        public string Json { get; set; }
        public string Template { get; set; }
        public string SqlStr { get; set; }
        public string SqlPart { get; set; }
    }
}
