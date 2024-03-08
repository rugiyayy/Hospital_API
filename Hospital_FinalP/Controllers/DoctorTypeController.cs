using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.DoctorType;
using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorTypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DoctorTypeController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int? page, int? perPage, string typeName = null)
        {
            if (_context.DoctorTypes == null) return NotFound();

            IQueryable<DoctorType> query = _context.DoctorTypes;

            if (!string.IsNullOrEmpty(typeName))
            {
                query = query.Where(a => a.Name.Contains(typeName));
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

            var types = await query
               .Select(x => _mapper.Map(x, new DoctorTypeGetDto()))
               .AsNoTracking()
               .ToListAsync();

            var tDta = await _context.DoctorTypes
                       .AsNoTracking()
                       .ToListAsync();


            return Ok(new { types , totalCount , tDta });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (_context.DoctorTypes == null) return NotFound();

            var type = _context.DoctorTypes
                .FirstOrDefault(x => x.Id == id);
            if (type is null) return NotFound();

            var dto = new DoctorTypeGetDto();
            _mapper.Map(type, dto);

            return Ok(dto);
        }


        [HttpPost]
        [Authorize(Roles = "Scheduler,Admin")]

        public IActionResult Post([FromBody] DoctorTypePostDto dto)
        {
            if (_context.DoctorTypes.Any(d => d.Name == dto.Name))
            {
                return Conflict($"Type with the name '{dto.Name}' already exists.");
            } 
            var type = new DoctorType();
            type = _mapper.Map(dto, type);

            _context.Add(type);
            _context.SaveChanges();

            return Ok(type.Id);
        }

        [HttpPut("{id}")]

        [Authorize(Roles = "Scheduler,Admin")]

        public IActionResult Put(int id, [FromBody] DoctorTypePutDto dto) 
        {
            var type = _context.DoctorTypes.FirstOrDefault(x => x.Id == id);
            if (type is null) return NotFound();

            if (_context.DoctorTypes.Any(d => d.Name == dto.Name && d.Id != id))
            {
                return Conflict($"Type with the name '{dto.Name}' already exists.");
            }

            _mapper.Map(dto, type);

            _context.SaveChanges();

            return Ok(type.Id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id)
        {
            var type = _context.DoctorTypes.FirstOrDefault(x => x.Id == id);
            if (type is null) return NotFound();

            _context.Remove(type);
            _context.SaveChanges();

            return Ok();
        }
    }
}
