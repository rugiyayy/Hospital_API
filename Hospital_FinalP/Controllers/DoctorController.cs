using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public DoctorController(AppDbContext context, IMapper mapper , IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var doctorDto = _context.Doctors
                .Include(x => x.DoctorType)
                .Include(x => x.Department)
                .Include(x => x.DocPhoto)
                .Include(x=>x.DoctorDetail)
                .Include(x=>x.ExaminationRoom)
                .Select(x => _mapper.Map(x, new DoctorGetDto()))
                .AsNoTracking()
                .ToList();

            return Ok(doctorDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var doctor=_context.Doctors
                .Include(x => x.DoctorType)
                .Include(x => x.Department)
                .Include(x => x.DocPhoto)
                .Include(x => x.DoctorDetail)
                .Include(x => x.ExaminationRoom).FirstOrDefault(x => x.Id == id);


            if (doctor is null) return NotFound();

            var dto = _mapper.Map<DoctorGetDto>(doctor);
            _mapper.Map(doctor, dto);

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Post([FromForm] DoctorPostDto dto)
        {
            var doctorEntity = _mapper.Map<Doctor>(dto);
            _context.Add(doctorEntity);
           

            if (dto.Photo != null)
            {
                var imageName = _fileService.SaveImage(dto.Photo);
                doctorEntity.DocPhoto = new DocPhoto { PhotoPath = imageName };
               
            }
            _context.SaveChanges();

            return Ok();

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] DoctorPutDto dto)
        {
            var doctor = _context.Doctors
                .Include(x => x.DoctorType)
                .Include(x => x.Department)
                .Include(x => x.DocPhoto)
                .Include(x => x.DoctorDetail)
                .Include(x => x.ExaminationRoom).FirstOrDefault(x => x.Id == id);

            if (doctor is null) return NotFound();

            _mapper.Map(dto, doctor);

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
            var doctor =_context.Doctors
                .Include(d => d.DocPhoto)
                .FirstOrDefault(x=>x.Id == id);
            if (doctor is null) return NotFound();


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
