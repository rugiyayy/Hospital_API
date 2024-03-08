using Hospital_FinalP.Services.Abstract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital_FinalP.Services.Concrete
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string fullName, string userName, List<string> roles, int? patientId = null, int? doctorId = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, fullName),
        new Claim(ClaimTypes.Email, userName)
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (patientId.HasValue)
            {
                claims.Add(new Claim("PatientId", patientId.Value.ToString())); // Include patient ID claim
            }

            if (doctorId.HasValue)
            {
                claims.Add(new Claim("DoctorId", doctorId.Value.ToString())); 
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}



       



