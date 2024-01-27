using Microsoft.AspNetCore.Identity;

namespace Hospital_FinalP.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
