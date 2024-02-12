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

        public string GenerateToken(string fullName, string userName, List<string> roles, int? patientId = null)
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
        //        {
        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        //            var claims = new List<Claim>()
        //            {
        //                  new Claim("Id", id),
        //                new Claim("UserName", userName),
        //                new Claim("FullName", fullName),


        //            };

        //            if (roles != null)
        //            {
        //                // Add role claims only if user has roles
        //                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        //            }

        //            var token = new JwtSecurityToken(
        //                expires: DateTime.Now.AddMinutes(60),
        //                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        //                claims: claims);
        //            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //            return jwt;
        //        }


        //    }
        //}


        //using Hospital_FinalP.Entities;
        //using Hospital_FinalP.Services.Abstract;
        //using Microsoft.IdentityModel.Tokens;
        //using System.IdentityModel.Tokens.Jwt;
        //using System.Security.Claims;
        //using System.Text;

        //namespace Hospital_FinalP.Services.Concrete
        //{
        //    public class JwtTokenService : IJwtTokenService
        //    {
        //        private readonly IConfiguration _configuration;

        //        public JwtTokenService(IConfiguration configuration)
        //        {
        //            _configuration = configuration;
        //        }

        //        public string GenerateToken(string userId, string fullName, string userName, List<string> roles, int? patientId = null)
        //        {
        //            var tokenHandler = new JwtSecurityTokenHandler();

        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));


        //            var tokenDescriptor = new SecurityTokenDescriptor
        //            {
        //                Subject = new ClaimsIdentity(new[]
        //                {
        //                    new Claim(ClaimTypes.NameIdentifier, userId),
        //            new Claim(ClaimTypes.Name, userName),
        //            new Claim(ClaimTypes.Email, userName), // Assuming email is the username
        //            new Claim(ClaimTypes.Role, string.Join(",", roles)),
        //            new Claim("FullName", fullName),
        //            new Claim("PatientId", patientId)
        //                }),


        //                Expires = DateTime.UtcNow.AddHours(2),
        //                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)

        //            };

        //            //var claims = new List<Claim>()
        //            //{
        //            //      new Claim("Id", id),
        //            //    new Claim("UserName", userName),
        //            //    new Claim("FullName", fullName),


        //            //};

        //            //if (roles != null)
        //            //{
        //            //    // Add role claims only if user has roles
        //            //    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        //            //}

        //            //var token = new JwtSecurityToken(
        //            //    expires: DateTime.Now.AddMinutes(60),
        //            //    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        //            //    claims: claims);
        //            //var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //            var token = tokenHandler.CreateToken(tokenDescriptor);
        //            return tokenHandler.WriteToken(token);
        //        }


        //    }
        //}








       



