using System.ComponentModel.DataAnnotations;

namespace Hospital_FinalP.DTOs.Account
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 4 characters!")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
