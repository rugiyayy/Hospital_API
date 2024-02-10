using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Department;
using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_context.Departments == null) return NotFound();
            var departmentsDto = _context.Departments
                .Select(x => _mapper.Map(x, new DepartmentGetDto()))
                .AsNoTracking()
                .ToList();

            return Ok(departmentsDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (_context.Departments == null) return NotFound();

            var deparment = _context.Departments
                .FirstOrDefault(x => x.Id == id);


            if (deparment is null) return NotFound();

            var dto = new DepartmentGetDto();
            _mapper.Map(deparment, dto);

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] DepartmentPostDto dto)
        {
            if (_context.Departments.Any(d => d.Name == dto.Name))
            {
                return Conflict($"The department with the name '{dto.Name}' already exists. Please choose a different name for the department.");
            }

            var department = new Department();
            department = _mapper.Map(dto, department);

            _context.Add(department);
            _context.SaveChanges();

            return Ok(department.Id);

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DepartmentPutDto dto)
        {
            var department = _context.Departments.FirstOrDefault(x => x.Id == id);
            if (department is null) return NotFound();


            if (_context.Departments.Any(d => d.Name == dto.Name && d.Id != id))
            {
                return Conflict($"The department with the name '{dto.Name}' already exists. Please choose a different name for the department.");
            }

            _mapper.Map(dto, department);

            _context.SaveChanges();

            return Ok(department.Id);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var department = _context.Departments.FirstOrDefault(x => x.Id == id);
            if (department is null) return NotFound();

            _context.Remove(department);
            _context.SaveChanges();

            return Ok();

        }
    }
}
