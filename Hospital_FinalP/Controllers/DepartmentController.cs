using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Department;
using Hospital_FinalP.DTOs.DoctorType;
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
        public async Task<IActionResult> Get(int? page, int? perPage, string departmentName = null)
        {
            if (_context.Departments == null) return NotFound();

            IQueryable<Department> query = _context.Departments;

            if (!string.IsNullOrEmpty(departmentName))
            {
                query = query.Where(a => a.Name.Contains(departmentName));
            }

            int totalCount = await query.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                query = query.OrderBy(a => a.Name).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                query = query.OrderBy(a => a.Name);
            }

            var departments = await query
               .Select(x => _mapper.Map(x, new DepartmentGetDto()))
               .AsNoTracking()
               .ToListAsync();

            return Ok(new { departments, totalCount });
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
