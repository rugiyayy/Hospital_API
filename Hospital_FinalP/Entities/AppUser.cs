using Microsoft.AspNetCore.Identity;

namespace Hospital_FinalP.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpires { get; set; }

    }



}
