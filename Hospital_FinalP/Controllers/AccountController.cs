using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Services.Abstract;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("GetUsersByRole")]
        public async Task<IActionResult> GetUsersByRole(int? page, int? perPage)
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var schedulerUsers = await _userManager.GetUsersInRoleAsync("Scheduler");

            var adminUserDtos = _mapper.Map<List<UserDto>>(adminUsers);
            var schedulerUserDtos = _mapper.Map<List<UserDto>>(schedulerUsers);

            foreach (var adminUserDto in adminUserDtos)
            {
                adminUserDto.IsAdmin = true;
            }

            foreach (var schedulerUserDto in schedulerUserDtos)
            {
                schedulerUserDto.IsAdmin = false;
            }

            var allUsers = adminUserDtos.Concat(schedulerUserDtos);

            int totalCount = allUsers.Count();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                allUsers = allUsers.Skip(skip).Take(itemsPerPage);
            }

            return Ok(new {allUsers, totalCount });


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userDto.IsAdmin = roles.Contains("Admin"); 

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }
        }


        [AllowAnonymous]

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
        [Authorize(Roles = "Admin")]

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



        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.PasswordResetLinkUsed = false;
            await _userManager.UpdateAsync(user);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{model.FrontendPort}/resetPassword?email={model.Email}&token={Uri.EscapeDataString(token)}";

            await SendPasswordResetEmailAsync(user.Email, resetLink);

            return Ok("Password reset link sent successfully");
        }

        private async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("REY Hospital", "demo1flight@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Reset Your Password";

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<p>Please click the following link to reset your password:</p><p><a href=\"{resetLink}\">Reset Password</a></p>";


            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("demo1flight@gmail.com", "crvglwtaqkfpejxv");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }




        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto model)
        {
           

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.PasswordResetLinkUsed)
            {
                return BadRequest("Link has already been used");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                user.PasswordResetLinkUsed = true;
                await _userManager.UpdateAsync(user);
                return Ok("Password reset successfully");
            }
            else
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }
        }

    }

}
