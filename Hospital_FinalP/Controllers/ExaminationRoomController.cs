using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationRoomController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] AppDbContext context)
        {
            if (context.ExaminationRooms == null) return NotFound("Not Fpund any rooms");

            var examinationRooms = await context.ExaminationRooms.Include(d=>d.Doctor).Select(a=>new
            {
                RoomId = a.Id,
                RoomNumber = a.RoomNumber,
                DoctorName = a.Doctor.FullName,
                Department=a.Doctor.Department.Name,

            })
                   .AsNoTracking()
                   .ToListAsync();

            return Ok(examinationRooms);
        }
    }
}
