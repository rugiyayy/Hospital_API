using Microsoft.AspNetCore.Identity;

namespace Hospital_FinalP.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool PasswordResetLinkUsed { get; set; }

    }

}
