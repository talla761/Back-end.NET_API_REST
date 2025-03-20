using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P7CreateRestApi.DTOs
{
    public class UserDTO
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incrémentation de l'ID
        //public int Id { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
    }
}
