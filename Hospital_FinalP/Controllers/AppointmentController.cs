using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;



namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
           
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments(int? page,int? perPage,string searchQuery = null,bool? isActive = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                 .Include(a => a.Doctor.Department);

            DateTime currentDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            if (isActive.HasValue)
            {
                if (isActive.Value)
                {
                    query = query.Where(a => a.IsActive && a.StartTime >= currentDateTimeLocal);
                }
                else 
                {
                    query = query.Where(a => !a.IsActive || a.EndTime < currentDateTimeLocal);
                }
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(a => a.Doctor.FullName.Contains(searchQuery) || a.Patient.FullName.Contains(searchQuery));
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(a => a.StartTime.Date >= startDate.Value.Date && a.EndTime.Date <= endDate.Value.Date);
            }
            else if (startDate.HasValue)
            {
                query = query.Where(a => a.StartTime.Date >= startDate.Value.Date);     
            }
            else if (endDate.HasValue)
            {
                query = query.Where(a => a.EndTime.Date <= endDate.Value.Date);
            }

            int totalCount = await query.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                query = query.OrderBy(a => a.StartTime).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                query = query.OrderBy(a => a.StartTime);
            }


            var appointments = await query
                .Select(x => _mapper.Map(x, new AppointmentGetDto
                {
                    ServiceCost=x.Doctor.Department.ServiceCost,
                    DoctorEmail = x.Doctor.DoctorDetail.Email,
                    Department = x.Doctor.Department.Name, 
                    Type = x.Doctor.DoctorType.Name,

                }))
                .AsNoTracking()
                .ToListAsync();

            var apps = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { totalCount, appointments, apps });
        }



        [HttpGet("patients/{patientId}")]
        public async Task<IActionResult> GetAppointmentsForPatient(int patientId, int? page,int? perPage, bool? isActive = null)
        {
            if (_context.Appointments == null) return NotFound();

            IQueryable<Appointment> appointmentsQuery = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.StartTime);


            if (isActive.HasValue)
            {
                if (isActive.HasValue)
                {
                    DateTime currentDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

                    if (isActive.Value)
                    {
                        appointmentsQuery = appointmentsQuery.Where(a => a.IsActive && a.StartTime >= currentDateTimeLocal);
                    }
                    else
                    {
                        appointmentsQuery = appointmentsQuery.Where(a => !a.IsActive || a.StartTime < currentDateTimeLocal);
                    }
                }

            }


            int totalCount = await appointmentsQuery.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                appointmentsQuery = appointmentsQuery.OrderBy(a => a.StartTime).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                appointmentsQuery = appointmentsQuery.OrderBy(a => a.StartTime);
            }



            var appointments = await appointmentsQuery
                .Select(x => _mapper.Map(x, new AppointmentGetDto
                {
                    DoctorEmail = x.Doctor.DoctorDetail.Email,
                    Department=x.Doctor.Department.Name,
                    Type=x.Doctor.DoctorType.Name,

                }))
                .AsNoTracking()
                .ToListAsync();



            return Ok(new { appointments, totalCount });


        }




        [HttpGet("doctors/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsForDoctor(int doctorId,int? page, int? perPage, bool? isActive = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (_context.Appointments == null) return NotFound();

            IQueryable<Appointment> appointmentsQuery = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.StartTime);


            if (isActive.HasValue)
            {
                if (isActive.Value) 
                {
                    appointmentsQuery = appointmentsQuery.Where(a => a.IsActive && a.StartTime >= DateTime.UtcNow);
                }
                else 
                {
                    appointmentsQuery = appointmentsQuery.Where(a => !a.IsActive || (a.IsActive && a.StartTime < DateTime.UtcNow));
                }
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.StartTime.Date >= startDate.Value.Date && a.EndTime.Date <= endDate.Value.Date);
            }
            else if (startDate.HasValue)
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.StartTime.Date >= startDate.Value.Date);
            }
            else if (endDate.HasValue)
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.EndTime.Date <= endDate.Value.Date);
            }



            int totalCount = await appointmentsQuery.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                appointmentsQuery = appointmentsQuery.OrderBy(a => a.StartTime).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                appointmentsQuery = appointmentsQuery.OrderBy(a => a.StartTime);
            }



            var appointments = await appointmentsQuery
                .Select(x => _mapper.Map(x, new AppointmentGetDto()))
                .AsNoTracking()
                .ToListAsync();

            var apps=await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .AsNoTracking()
                .ToListAsync();


            return Ok(new { appointments, totalCount, apps });


        }



        [HttpGet("todaysApp/{doctorId}")]
        public async Task<IActionResult> DoctorsTodaysAppointments(int doctorId, int? page, int? perPage)
        {

            if (_context.Appointments == null) return NotFound();

            DateTime today = DateTime.Today;

            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.StartTime.Date == today)
                .OrderBy(a => a.StartTime);

            int totalCount = await query.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                query = query.OrderBy(a => a.StartTime).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                query = query.OrderBy(a => a.StartTime);
            }


            var appointments = await query
                .Select(x => _mapper.Map(x, new AppointmentGetDto()))
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { totalCount, appointments });
        }

   




    [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
               .Include(a => a.Doctor)
               .Include(a => a.Patient)
               .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound("Appointment not found.");

            bool isActive = appointment.StartTime >= DateTime.UtcNow;

            var appointmentDto = _mapper.Map<AppointmentGetDto>(appointment);
            appointmentDto.IsActive = isActive;



            return Ok(appointmentDto);
        }





        [Authorize(Roles = "Patient, Scheduler,Admin")]
        [HttpPost]
        public async Task<IActionResult> ScheduleAppointment([FromBody] AppointmentPostDto dto)
        {
            var doctor = await _context.Doctors
                .Include(d => d.WorkingSchedule)
                 .FirstOrDefaultAsync(d => d.Id == dto.DoctorId);

            if (doctor == null) return NotFound("Doctor not found.");

            if (doctor.WorkingSchedule == null) return BadRequest("WorkingSchedule is not set.");

            var patient = await _context.Patients.FindAsync(dto.PatientId);
            if (patient == null) return NotFound("Patient not found.");


            TimeSpan duration = TimeSpan.FromMinutes(30);

            if (dto.StartTime.TimeOfDay < doctor.WorkingSchedule.StartTime || dto.StartTime.TimeOfDay.Add(duration) > doctor.WorkingSchedule.EndTime)
            {
                return BadRequest("Appointment time is outside of doctor's working schedule.");
            }
            if (dto.StartTime.Hour == 14 && (dto.StartTime.Minute == 0 || dto.StartTime.Minute == 30))
            {
                return BadRequest("Appointment time coincides with the lunch break.");
            }

            if (dto.StartTime.Minute % 30 != 0) return BadRequest("Appointment start time should be at intervals of 30 minutes.");


            if (doctor.AvailableAppointments <= 0) return BadRequest("No available appointments for this doctor at the specified time.");


            DateTime endTime = dto.StartTime.Add(duration);



            var existingPatientAppointment = await _context.Appointments
                   .FirstOrDefaultAsync(a => a.PatientId == dto.PatientId &&
                                              a.StartTime == dto.StartTime);

            if (existingPatientAppointment != null) return Conflict("Patient already has an appointment at the specified time.");


            var existingDoctorAppointment = await _context.Appointments
        .FirstOrDefaultAsync(a => a.DoctorId == dto.DoctorId &&
                                   a.StartTime == dto.StartTime);

            if (existingDoctorAppointment != null) return Conflict("Doctor already has an appointment at the specified time.");


            var currentDateTimeUtc = DateTime.UtcNow;

            DateTime currentDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(currentDateTimeUtc, TimeZoneInfo.Local);

            if (dto.StartTime < currentDateTimeLocal) return BadRequest("Appointment time cannot be in the past.");



            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                StartTime = dto.StartTime,
                Duration = duration,
                Description = dto.Description,
                IsActive = true
            };

            _context.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(appointment);

        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Scheduler,Admin")]

        public async Task<IActionResult> DeleteAppointment(int id)
        {

            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                return NotFound("Appointment not found.");


            _context.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok();
        }




       


        [HttpGet("AvailableTimeSlots")]
        public IActionResult GetAvailableTimeSlots(string selectedDate, int doctorId)
        {
            if (!DateTime.TryParse(selectedDate, out DateTime parsedDate))
                return BadRequest("Invalid date format. Please provide the date in YYYY-MM-DD format.");

            DateTime currentDateTimeUtc = DateTime.UtcNow;
            DateTime currentDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(currentDateTimeUtc, TimeZoneInfo.Local);

            DateTime today = currentDateTimeLocal.Date;
            TimeSpan currentTime = currentDateTimeLocal.TimeOfDay;

            DateTime lastDateAllowed = currentDateTimeLocal.AddDays(30);

            if (parsedDate < today || parsedDate > lastDateAllowed)
                return BadRequest("Selected date must be within the next 30 days from today.");

            if (parsedDate.Date < today)
                parsedDate = today;

            var workingSchedule = _context.WorkingSchedules
                .Include(ws => ws.WorkingDays)
                .FirstOrDefault(ws => ws.DoctorId == doctorId);

            if (workingSchedule == null)
                return BadRequest("Working schedule not found for the specified doctor.");

            var parsedDayOfWeek = parsedDate.DayOfWeek;
            var isWorkingDay = workingSchedule.WorkingDays.Any(wd => wd.DayOfWeek == parsedDayOfWeek);

            if (!isWorkingDay)
                return BadRequest("Working schedule not found for the specified day.");

            TimeSpan interval = new TimeSpan(0, 30, 0);
            List<DateTime> timeSlots = GenerateTimeSlots(parsedDate, workingSchedule.StartTime, workingSchedule.EndTime, interval);

            var occupiedTimeSlots = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.StartTime.Date == parsedDate.Date)
                .Select(a => a.StartTime.TimeOfDay)
                .ToList();

            timeSlots = timeSlots
                .Where(ts => !occupiedTimeSlots.Contains(ts.TimeOfDay))
                .ToList();

            if (parsedDate.Date == today)
            {
                timeSlots = timeSlots.Where(ts => ts.TimeOfDay >= currentTime).ToList();
            }

            List<string> formattedTimeSlots = timeSlots.Select(ts => ts.ToString("HH:mm")).ToList();

            return Ok(formattedTimeSlots);
        }


        

        private List<DateTime> GenerateTimeSlots(DateTime selectedDate, TimeSpan startTime, TimeSpan endTime, TimeSpan interval)
        {
            List<DateTime> timeSlots = new List<DateTime>();
            DateTime currentTimeSlot = selectedDate.Date + startTime;
            TimeSpan lunchStartTime = new TimeSpan(14, 0, 0);
            TimeSpan lunchEndTime = new TimeSpan(15, 0, 0); 

            while (currentTimeSlot.TimeOfDay < endTime)
            {
                if (!(currentTimeSlot.TimeOfDay >= lunchStartTime && currentTimeSlot.TimeOfDay < lunchEndTime))
                {
                    timeSlots.Add(currentTimeSlot);
                }
                currentTimeSlot = currentTimeSlot.Add(interval);
            }

            return timeSlots;
        }


    }
}
