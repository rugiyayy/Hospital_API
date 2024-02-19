﻿using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Services.Abstract;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var token = jwtTokenService.GenerateToken( user.FullName, user.UserName, roles, patientId,doctorId);
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

            var result = await _userManager.CreateAsync(userEntity,dto.Password);
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

            //kto mojet zareqat pachienta ?: 
            // appoitm scheduler ( u neqo toje nudet svoya str i tp , ottudai  smojet zareqat admina i tp v sluchae cheqo .
            //sam pachient

            // u appoitm schedulera toje budet sign up str otdelnaya chtobi on tam reqal patienta for appoinmet 

        }

    }
}
