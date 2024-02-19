using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public DoctorController(AppDbContext context, IMapper mapper, [FromServices] IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult GetAll(/*[FromQuery(Name = "_page")] int page, [FromQuery(Name = "_perPage")]  int perPage*/)
        {
            //    page = (page <= 0) ? 1 : page;

            //    decimal doctorCount = _context.Doctors.Count();


            //    int totalPages = (int)Math.Ceiling(doctorCount / perPage);


            //var doctorDto = _context.Doctors
            //    .Include(x => x.DoctorType)
            //    .Include(x => x.Department)
            //    .Include(x => x.DocPhoto)
            //    .Include(x => x.DoctorDetail)
            //    .Include(x => x.ExaminationRoom);

            //  //.OrderBy(x => x.Id)
            //  //  .Skip((page - 1) * perPage)
            //  //  .Take(perPage)
            //  //  .AsNoTracking()
            //  //  .ToList();

            //var dto = doctorDto.Select(x => _mapper.Map(x, new DoctorGetDto
            //{
            //    DoctorTypeName = x.DoctorType.Name,
            //    DepartmentName = x.Department.Name,

            //}));

            //return Ok(dto);

                var doctors = _context.Doctors
                        .Include(x => x.DoctorType)
                        .Include(x => x.Department)
                        .Include(x => x.DocPhoto)
                        .Include(x => x.DoctorDetail)
                        .Include(x => x.ExaminationRoom)
                        .AsNoTracking()
                        .ToList();


              var doctorDto = doctors
                         .Select(x => _mapper.Map(x, new DoctorGetDto
                         {
                             DoctorTypeName = x.DoctorType.Name,
                             DepartmentName = x.Department.Name,
                             ServiceCost = x.Department.ServiceCost,
                         }))
                         .OrderBy(x => x.FullName)
                         .ToList();

            return Ok(doctorDto);


        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var doctor = _context.Doctors
                .Include(x => x.DoctorType)
                .Include(x => x.Department)
                .Include(x => x.DocPhoto)
                .Include(x => x.DoctorDetail)
                .Include(x => x.ExaminationRoom).FirstOrDefault(x => x.Id == id);


            if (doctor is null) return NotFound("Doctor Not Found");

            var dto = _mapper.Map<DoctorGetDto>(doctor);
            _mapper.Map(doctor, dto);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] DoctorPostDto dto, [FromServices] UserManager<AppUser> userManager)
        {
            var existingDoctor = _context.Doctors
                    .AsEnumerable()
                    .FirstOrDefault(d => d.FullName.Trim().Equals(dto.FullName.Trim(), StringComparison.OrdinalIgnoreCase));


            if (existingDoctor != null)
            {
                return Conflict("Doctor with the same FullName already exists.");
            }


            var existingDepartment = _context.Departments.FirstOrDefault(dep => dep.Id == dto.DepartmentId);
            if (existingDepartment == null)
            {

                return BadRequest($"Department with Id {dto.DepartmentId} does not exist.");
            }

            var existingDoctorType = _context.DoctorTypes.FirstOrDefault(dt => dt.Id == dto.DoctorTypeId);
            if (existingDoctorType == null)
            {
                return BadRequest($"DoctorType with Id {dto.DoctorTypeId} does not exist.");
            }



            var existingExaminationRoom = _context.ExaminationRooms
           .FirstOrDefault(er => er.RoomNumber == dto.ExaminationRoom.RoomNumber);

            if (existingExaminationRoom != null)
            {
                return Conflict($"Examination Room with Room Number {dto.ExaminationRoom.RoomNumber} is already associated with another doctor.");
            }


            var doctorEntity = _mapper.Map<Doctor>(dto);

            doctorEntity.WorkingSchedule = new WorkingSchedule
            {
                StartTime = dto.WorkingSchedule.StartTime != null ? TimeSpan.Parse(dto.WorkingSchedule.StartTime) : TimeSpan.FromHours(9), 
                EndTime = dto.WorkingSchedule.EndTime != null ? TimeSpan.Parse(dto.WorkingSchedule.EndTime) : TimeSpan.FromHours(18), 
                WorkingDays = dto.WorkingSchedule.WorkingDays.Select(d => new WorkingDay { DayOfWeek = d.DayOfWeek }).ToList()
            };









            doctorEntity.MaxAppointments = doctorEntity.CalculateMaxAppointments(TimeSpan.FromMinutes(30)); // Assuming appointment duration is 30 minutes



            _context.Add(doctorEntity);


            if (dto.Photo != null)
            {
                var imageName = _fileService.SaveImage(dto.Photo);
                doctorEntity.DocPhoto = new DocPhoto { PhotoPath = imageName };

            }





            //{
            //    // Check if the Department and DoctorType exist before saving the photo
            //    if (existingDepartment != null && existingDoctorType != null)
            //    {
            //        var imageName = _fileService.SaveImage(dto.Photo);
            //        doctorEntity.DocPhoto = new DocPhoto { PhotoPath = imageName };
            //    }
            //    else
            //    {
            //        return BadRequest("Department or DoctorType does not exist. Photo not added.");
            //    }
            //}




            _context.SaveChanges();
            var docUser = new AppUser { UserName = dto.DoctorDetail.Email, Email = dto.DoctorDetail.Email, FullName = dto.FullName };
            var result = await userManager.CreateAsync(docUser, dto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(docUser, "Doctor");
            }
            else
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }

            return Ok("Doctor and Doctor Account created successfully");

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] DoctorPutDto dto)
        {


            var existingDoctorWithEmail = _context.DoctorDetails
       .Where(dd => dd.DoctorId != id)
       .AsEnumerable()  // Switch to client evaluation
       .FirstOrDefault(d => d.Email.Equals(dto.DoctorDetail.Email, StringComparison.OrdinalIgnoreCase));

            if (existingDoctorWithEmail != null)
            {
                return BadRequest($"Email {dto.DoctorDetail.Email} is already associated with another doctor.");
            }
            var doctor = _context.Doctors
                .Include(x => x.DocPhoto)
                .Include(x => x.DoctorDetail)
                .Include(x => x.ExaminationRoom).FirstOrDefault(x => x.Id == id);

            if (doctor is null) return NotFound();



            if (dto.DoctorDetail != null)
            {
                doctor.DoctorDetail = new DoctorDetail
                {
                    PhoneNumber = dto.DoctorDetail.PhoneNumber,
                    Email = dto.DoctorDetail.Email,
                };
            }





            //_mapper.Map(dto, doctor);

            if (doctor.DocPhoto != null)
            {
                //deleting old one
                _fileService.DeleteFile(doctor.DocPhoto.PhotoPath);
            }

            if (dto.Photo != null)
            {
                var imageName = _fileService.SaveImage(dto.Photo);
                doctor.DocPhoto = new DocPhoto { PhotoPath = imageName };


            }

            _context.SaveChanges();


            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var doctor = _context.Doctors
                .Include(d => d.DocPhoto)
                .FirstOrDefault(x => x.Id == id);
            if (doctor is null) return NotFound("Doctor Not Found");


            if (doctor.DocPhoto != null)
            {
                _fileService.DeleteFile(doctor.DocPhoto.PhotoPath);
            }

            _context.Remove(doctor);
            _context.SaveChanges();
            return Ok();

        }
    }
}
