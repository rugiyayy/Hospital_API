﻿using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Department;
using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.DTOs.Patients;
using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PatientController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_context.Patients == null) return NotFound();
            var patientssDto = _context.Patients
                .Select(x => _mapper.Map(x, new PatientGetDto()))
                .AsNoTracking()
                .ToList();

            return Ok(patientssDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var patient = _context.Patients.FirstOrDefault(x => x.Id == id);
            if (patient is null) return NotFound("Patient not found");

            var dto = new PatientGetDto();
            _mapper.Map(patient, dto);

            return Ok(dto);
        }

        [HttpPost]//register patient
        public async Task<IActionResult> PatientSignUp([FromBody] PatientPostDto dto, [FromServices] UserManager<AppUser> userManager)
        {
            var existingPatient = _context.Patients
                    .AsEnumerable()
                      .FirstOrDefault(d => d.PatientIdentityNumber.Trim().Equals(dto.PatientIdentityNumber.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existingPatient != null)
            {
                return Conflict("Patient already has an account.");
            }


            var existingEmail = _context.Patients
                .Where(dd => dd.PatientIdentityNumber != dto.PatientIdentityNumber)
                .AsEnumerable()
                .FirstOrDefault(d => d.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));


            if (existingEmail != null)
            {
                return BadRequest($"Email {dto.Email} is already associated with another patient.");
            }


            var existingNumber = _context.Patients
                 .Where(dd => dd.PatientIdentityNumber != dto.PatientIdentityNumber)
              .AsEnumerable()
             .FirstOrDefault(d => d.PhoneNumber.Trim().Equals(dto.PhoneNumber.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existingNumber != null)
            {
                return Conflict($" Phone number {dto.PhoneNumber} is already associated with another patient");
            }



            var patientEntity = _mapper.Map<Patient>(dto);
           


            var patientUser = new AppUser { UserName = dto.Email, Email = dto.Email, FullName = dto.FullName };
            var result = await userManager.CreateAsync(patientUser, dto.Password);

            if (result.Succeeded)
            {
                _context.Add(patientEntity);
                _context.SaveChanges();
                await userManager.AddToRoleAsync(patientUser, "Patient");

            }
            else
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }

            return Ok("Patient and Patient Account created successfully");

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PatientPutDto dto)
        {
            var patient = _context.Patients.FirstOrDefault(x => x.Id == id);
            if (patient is null) return NotFound("Patient not found");

            var existingPhoneNumber = _context.Patients
       .Where(dd => dd.Id != id)
       .AsEnumerable()
       .FirstOrDefault(d => d.PhoneNumber.Equals(dto.PhoneNumber, StringComparison.OrdinalIgnoreCase));

            if (existingPhoneNumber != null)
            {
                return BadRequest($"Phone number {dto.PhoneNumber} is already associated with another patient.");
            }
    

            var existingEmail = _context.Patients
                .Where(dd => dd.Id != id)
                .AsEnumerable()
                .FirstOrDefault(d => d.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

            if (existingEmail != null)
            {
                return BadRequest($"Email {dto.Email} is already associated with another patient.");
            }


            _mapper.Map(dto, patient);
            _context.SaveChanges();

            return Ok(patient.Id);

        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var patient = _context.Patients.FirstOrDefault(x => x.Id == id);
            if (patient is null) return NotFound("Patient Not Found");

            _context.Remove(patient);
            _context.SaveChanges();

            return Ok();

        }
    }
}
