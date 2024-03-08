using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Patients;
using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserManager<AppUser> _userManager;

        public PatientController(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? page, int? perPage, string patientFullName = null)
        {
            if (_context.Patients == null) return NotFound();

            IQueryable<Patient> query = _context.Patients;

            if (!string.IsNullOrEmpty(patientFullName))
            {
                query = query.Where(a => a.FullName.Contains(patientFullName));
            }

            int totalCount = await query.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                query = query.OrderBy(a => a.FullName).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                query = query.OrderBy(a => a.FullName);
            }

            var patients = await query
               .Select(x => _mapper.Map(x, new PatientGetDto()))
               .AsNoTracking()
               .ToListAsync();


            var pDta = await _context.Patients
                        .AsNoTracking()
                        .ToListAsync();


            return Ok(new { patients, pDta, totalCount});
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
        [AllowAnonymous]


        public async Task<IActionResult> PatientSignUp([FromBody] PatientPostDto dto)
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
            var result = await _userManager.CreateAsync(patientUser, dto.Password);

            if (result.Succeeded)
            {
                _context.Add(patientEntity);
                _context.SaveChanges();
                await _userManager.AddToRoleAsync(patientUser, "Patient");

            }
            else
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }

            return Ok("Patient and Patient Account created successfully");

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Scheduler,Admin")]

        public async Task<IActionResult> Update(int id, [FromBody] PatientPutDto dto)
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


            var user = await _userManager.FindByEmailAsync(patient.Email);

            if (user != null)
            {
                user.Email = dto.Email;
                user.UserName = dto.Email;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return BadRequest(updateResult.Errors.Select(error => error.Description));
                }
            }

            _mapper.Map(dto, patient);

            await _context.SaveChangesAsync();

            return Ok(patient.Id);

        }



        [HttpDelete("{id}")]

        [Authorize(Roles = "Admin")]  

        public async Task<IActionResult> Delete(int id)
        {

            var patient = _context.Patients.FirstOrDefault(x => x.Id == id);
            if (patient is null) return NotFound("Patient Not Found");

            var user = await _userManager.FindByEmailAsync(patient.Email);
            if (user != null)
            {
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    return BadRequest(deleteResult.Errors.Select(error => error.Description));
                }
            }

            _context.Remove(patient);
            await _context.SaveChangesAsync();


            return Ok();

        }
    }
}
