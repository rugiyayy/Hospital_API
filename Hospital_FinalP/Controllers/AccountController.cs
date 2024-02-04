using AutoMapper;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;



        public AccountController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto dto, [FromServices] IJwtTokenService jwtTokenService)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, dto.Password)))
            {
                return Unauthorized("Invalid credentials");
            }

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var token = jwtTokenService.GenerateToken(user.FullName, user.UserName, roles);
            return Ok(token);
        }

        [HttpPost("SignUp")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto dto)
        {
            var userEntity = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(userEntity, dto.Password);

            if (result.Succeeded)
            {
                if (dto.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(userEntity, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(userEntity, "Appointment Scheduler");
                }

                return Ok("Account created successfully");
            }
            else
            {
                return BadRequest();
            }
            //kto mojet zareqat pachienta ?: 
            // appoitm scheduler ( u neqo toje nudet svoya str i tp , ottudai  smojet zareqat admina i tp v sluchae cheqo .
            //sam pachient

            // u appoitm schedulera toje budet sign up str otdelnaya chtobi on tam reqal patienta for appoinmet 

        }

    }
}
