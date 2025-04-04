using Microsoft.AspNetCore.Identity;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        //public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? Role { get; set; }
    }
}