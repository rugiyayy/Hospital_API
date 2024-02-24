﻿using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Migrations;
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

        //[HttpGet]
        //public IActionResult GetAll(/*[FromQuery(Name = "_page")] int page, [FromQuery(Name = "_perPage")]  int perPage*/)
        //{
        //    //    page = (page <= 0) ? 1 : page;

        //    //    decimal doctorCount = _context.Doctors.Count();


        //    //    int totalPages = (int)Math.Ceiling(doctorCount / perPage);


        //    //var doctorDto = _context.Doctors
        //    //    .Include(x => x.DoctorType)
        //    //    .Include(x => x.Department)
        //    //    .Include(x => x.DocPhoto)
        //    //    .Include(x => x.DoctorDetail)
        //    //    .Include(x => x.ExaminationRoom);

        //    //  //.OrderBy(x => x.Id)
        //    //  //  .Skip((page - 1) * perPage)
        //    //  //  .Take(perPage)
        //    //  //  .AsNoTracking()
        //    //  //  .ToList();

        //    //var dto = doctorDto.Select(x => _mapper.Map(x, new DoctorGetDto
        //    //{
        //    //    DoctorTypeName = x.DoctorType.Name,
        //    //    DepartmentName = x.Department.Name,

        //    //}));

        //    //return Ok(dto);

        //        var doctors = _context.Doctors
        //                .Include(x => x.DoctorType)
        //                .Include(x => x.Department)
        //                .Include(x => x.DocPhoto)
        //                .Include(x => x.DoctorDetail)
        //                .Include(x => x.ExaminationRoom)
        //                .AsNoTracking()
        //                .ToList();


        //      var doctorDto = doctors
        //                 .Select(x => _mapper.Map(x, new DoctorGetDto
        //                 {
        //                     DoctorTypeName = x.DoctorType.Name,
        //                     DepartmentName = x.Department.Name,
        //                     ServiceCost = x.Department.ServiceCost,
        //                 }))
        //                 .OrderBy(x => x.FullName)
        //                 .ToList();

        //    return Ok(doctorDto);


        //}

        //paging filtering etc 


        [HttpGet]
        public async Task<IActionResult> GetAll(int? page, int? perPage, string doctorTypeName = null, string departmentName = null, string doctorName = null, int? examinationRoomNumber = null)
        {
            IQueryable<Doctor> query = _context.Doctors
                        .Include(x => x.DoctorType)
                        .Include(x => x.Department)
                        .Include(x => x.DoctorDetail)
                        .Include(x => x.ExaminationRoom);

           

            if (!string.IsNullOrEmpty(doctorName))
            {
                query = query.Where(a => a.FullName.Contains(doctorName));
            }
            if (!string.IsNullOrEmpty(doctorTypeName))
            {
                query = query.Where(a => a.DoctorType.Name.Contains(doctorTypeName));
            }
            if (!string.IsNullOrEmpty(departmentName))
            {
                query = query.Where(a => a.Department.Name.Contains(departmentName));
            }
            if (examinationRoomNumber.HasValue)
            {
                query = query.Where(a => a.ExaminationRoom.RoomNumber == examinationRoomNumber.Value);
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


            var doctors = await query
                .Select(x => _mapper.Map(x, new DoctorGetDto
                {
                    DoctorTypeName = x.DoctorType.Name,
                    DepartmentName = x.Department.Name,
                    ServiceCost = x.Department.ServiceCost,
                    ExaminationRoom = new ExaminationRoomGetDto
                    {
                        RoomNumber = x.ExaminationRoom.RoomNumber,
                    }
                }))
                .AsNoTracking()
                .ToListAsync();



            //var examinationRooms = await _context.ExaminationRooms
            //        .AsNoTracking()
            //        .ToListAsync();

            return Ok(new { totalCount, doctors });

        }



        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var doctor = _context.Doctors
                .Include(x => x.DoctorType)
                .Include(x => x.Department)
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
            #region ErrorHandling
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

            #endregion


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
                doctorEntity.PhotoPath = imageName;

            }

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


            _context.SaveChanges();
            return Ok("Doctor and Doctor Account created successfully");

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] DoctorPutDto dto)
        {


            var existingDoctorWithEmail = _context.DoctorDetails
       .Where(dd => dd.DoctorId != id)
       .AsEnumerable()  
       .FirstOrDefault(d => d.Email.Equals(dto.DoctorDetail.Email, StringComparison.OrdinalIgnoreCase));

            if (existingDoctorWithEmail != null)
            {
                return Conflict($"Email {dto.DoctorDetail.Email} is already associated with another doctor.");
            }


            var existingDoctorPhoneNumber = _context.DoctorDetails
     .Where(dd => dd.DoctorId != id)
     .AsEnumerable()
     .FirstOrDefault(d => d.PhoneNumber.Equals(dto.DoctorDetail.PhoneNumber, StringComparison.OrdinalIgnoreCase));

            if (existingDoctorPhoneNumber != null)
            {
                return Conflict($"Phone Number {dto.DoctorDetail.PhoneNumber} is already associated with another doctor.");
            }


            var doctor = _context.Doctors
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

            //if (doctor.PhotoPath != null)
            //{
            //    //deleting old one
            //    _fileService.DeleteFile(doctor.PhotoPath);
            //}

            //if (dto.Photo != null)
            //{
            //    var imageName = _fileService.SaveImage(dto.Photo);
            //    doctor.DocPhoto = new DocPhoto { PhotoPath = imageName };


            //}
            if (dto.Photo != null)
            {
                // Delete the existing photo
                if (doctor.PhotoPath != null)
                {
                    _fileService.DeleteFile(doctor.PhotoPath);
                }

                // Save the new photo
                var imageName = _fileService.SaveImage(dto.Photo);
                doctor.PhotoPath = imageName;
            } 

            _context.SaveChanges();


            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var doctor = _context.Doctors
                .FirstOrDefault(x => x.Id == id);
            if (doctor is null) return NotFound("Doctor Not Found");


            if (doctor.PhotoPath != null)
            {
                _fileService.DeleteFile(doctor.PhotoPath);
            }

            _context.Remove(doctor);
            _context.SaveChanges();
            return Ok();

        }
    }
}
