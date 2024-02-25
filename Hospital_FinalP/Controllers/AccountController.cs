using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Services.Abstract;
using Humanizer;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MimeKit;
using System.Security.Cryptography;

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [HttpGet("GetUsersByRole")]
        public async Task<IActionResult> GetUsersByRole()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var schedulerUsers = await _userManager.GetUsersInRoleAsync("Scheduler");

            var adminUserDtos = _mapper.Map<List<UserDto>>(adminUsers);
            var schedulerUserDtos = _mapper.Map<List<UserDto>>(schedulerUsers);

            var usersByRole = new
            {
                AdminUsers = adminUserDtos,
                SchedulerUsers = schedulerUserDtos
            };

            return Ok(usersByRole);
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
            var isPatient = roles.Contains("Patient");
            int? patientId = null;
            var isDoctor = roles.Contains("Doctor");
            int? doctorId = null;

            if (isPatient)
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == user.Email);
                if (patient != null)
                {
                    patientId = patient.Id;
                }
            }

            if (isDoctor)
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorDetail.Email == user.Email);
                if (doctor != null)
                {
                    doctorId = doctor.Id;
                }
            }
            var token = jwtTokenService.GenerateToken(user.FullName, user.UserName, roles, patientId, doctorId);
            return Ok(token);
        }

        [HttpPost("SignUp")]
        //[Authorize(Roles = "Admin,Patient")]

        public async Task<IActionResult> SignUp([FromBody] SignUpDto dto)
        {


            var existingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmail != null)
            {
                return BadRequest("Email address is already in use.");
            }


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
                    await _userManager.AddToRoleAsync(userEntity, "Scheduler");
                }

                return Ok("Account created successfully");
            }
            else
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }

        }



        //[HttpPost("SignOut")]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();

        //    return Ok();
        //}



        [HttpGet("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }


            string resetCode = GenerateResetCode(); 
            user.ResetCode = resetCode;
            user.ResetCodeExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            await SendResetPasswordEmail(user.Email, resetCode); 

            return Ok("Reset code sent to your email.");
        }



        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResettPassword(ResetPasswordRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetCode == request.Code);
            if (user == null || user.ResetCodeExpires < DateTime.Now)
            {
                return BadRequest("Invalid CODE.");
            }


            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password); 
            user.ResetCode = null;
            user.ResetCodeExpires = null;


            await _context.SaveChangesAsync();

            return Ok("Password successfully reset.");
        }




        private async Task SendResetPasswordEmail(string email, string code)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("REY Hospital", "demo1flight@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Password Reset";

            message.Body = new TextPart("plain")
            {
                Text = $"To reset your password, please use the following code: {code}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("demo1flight@gmail.com", "crvglwtaqkfpejxv");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private string GenerateResetCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }

}
