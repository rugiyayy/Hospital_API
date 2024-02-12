using AutoMapper;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.Entities;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentController(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _context.Appointments
         .Include(a => a.Doctor)
         .Include(a => a.Patient)
         .ToListAsync();

         
            return Ok(appointments);

        }

        [HttpGet("patients/{patientId}/appointments")]
        public async Task<IActionResult> GetAppointmentsForPatient(int patientId)
        {
            var appointmentsForPatient = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();

            return Ok(appointmentsForPatient);
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

            var appointmentDto = _mapper.Map<AppointmentGetDto>(appointment);
            return Ok(appointmentDto);
        }


        [Authorize(Roles = "Patient, Scheduler")]
        [HttpPost]
        public async Task<IActionResult> ScheduleAppointment([FromForm] AppointmentPostDto dto)
        {
            if (dto.StartTime.Date == DateTime.Today)
            {
                return BadRequest("Booking for today is not allowed.");
            }
            var doctor = await _context.Doctors
                .Include(d => d.WorkingSchedule)
                 .FirstOrDefaultAsync(d => d.Id == dto.DoctorId);
            if (doctor == null)
                return NotFound("Doctor not found.");
            if (doctor.WorkingSchedule == null)
                return BadRequest("WorkingSchedule is not set.");

            var patient = await _context.Patients.FindAsync(dto.PatientId);
            if (patient == null)
            {
                return NotFound("Patient not found.");
            }


            TimeSpan duration = TimeSpan.FromMinutes(30);


            if (dto.StartTime.TimeOfDay < doctor.WorkingSchedule.StartTime ||
        dto.StartTime.TimeOfDay.Add(duration) > doctor.WorkingSchedule.EndTime)
            {
                return BadRequest("Appointment time is outside of doctor's working schedule.");
            }

            if (dto.StartTime.Minute % 30 != 0)
            {
                return BadRequest("Appointment start time should be at intervals of 30 minutes.");
            }



            var existingAppointments = await _context.Appointments
       .Where(a => a.DoctorId == dto.DoctorId &&
                   a.StartTime.Date == dto.StartTime.Date)
       .ToListAsync();

            int bookedAppointmentsCount = existingAppointments.Count;



            if (doctor.AvailableAppointments <= 0)
            {
                return BadRequest("No available appointments for this doctor at the specified time.");
            }



            DateTime endTime = dto.StartTime.Add(duration);



            var existingPatientAppointment = await _context.Appointments
                   .FirstOrDefaultAsync(a => a.PatientId == dto.PatientId &&
                                              a.StartTime == dto.StartTime);

            if (existingPatientAppointment != null)
            {
                return Conflict("Patient already has an appointment at the specified time.");
            }



            var existingDoctorAppointment = await _context.Appointments
        .FirstOrDefaultAsync(a => a.DoctorId == dto.DoctorId &&
                                   a.StartTime == dto.StartTime);

            if (existingDoctorAppointment != null)
            {
                return Conflict("Doctor already has an appointment at the specified time.");
            }


            DateTime currentDateTimeUtc = DateTime.UtcNow;

            DateTime currentDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            // Проверяем, если выбранное время приема меньше текущего местного времени
            if (dto.StartTime < currentDateTimeLocal)
            {
                // Возвращаем ошибку, если выбранное время приема меньше текущего местного времени
                return BadRequest("Appointment time cannot be in the past.");
            }

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                StartTime = dto.StartTime,
                Duration = duration,
                Description = dto.Description
            };

            _context.Add(appointment);
            await _context.SaveChangesAsync();
            return Ok(appointment);

        }

        //[Authorize(Roles = "Patient, Receptionist")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromForm] AppointmentPutDto dto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound("Appointment not found.");




            var doctor = await _context.Doctors
                .Include(d => d.WorkingSchedule)
                .FirstOrDefaultAsync(d => d.Id == dto.DoctorId);
            if (doctor == null)
                return NotFound("Doctor not found.");
            if (doctor.WorkingSchedule == null)
                return BadRequest("WorkingSchedule is not set.");


            DateTime currentDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            // Проверяем, если новое время приема меньше текущего местного времени
            if (dto.StartTime < currentDateTimeLocal)
            {
                // Возвращаем ошибку, если новое время приема меньше текущего местного времени
                return BadRequest("Appointment time cannot be in the past.");
            }

            TimeSpan duration = TimeSpan.FromMinutes(30);

            if (dto.StartTime.TimeOfDay < doctor.WorkingSchedule.StartTime ||
                dto.StartTime.TimeOfDay.Add(duration) > doctor.WorkingSchedule.EndTime)
            {
                return BadRequest("Appointment time is outside of doctor's working schedule.");
            }

            if (dto.StartTime.Minute % 30 != 0)
            {
                return BadRequest("Appointment start time should be at intervals of 30 minutes.");
            }

            var existingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == dto.DoctorId && a.StartTime.Date == dto.StartTime.Date && a.Id != id)
                .ToListAsync();

            //int bookedAppointmentsCount = existingAppointments.Count;

            if (doctor.AvailableAppointments <= 0)
            {
                return BadRequest("No available appointments for this doctor at the specified time.");
            }

            //doctor.AvailableAppointments = doctor.MaxAppointments - bookedAppointmentsCount;


            var existingPatientAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.StartTime == dto.StartTime && a.Id != id);

            if (existingPatientAppointment != null)
            {
                return Conflict("Patient already has an appointment at the specified time.");
            }

            var existingDoctorAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.DoctorId == dto.DoctorId && a.StartTime == dto.StartTime && a.Id != id);

            if (existingDoctorAppointment != null)
            {
                return Conflict("Doctor already has an appointment at the specified time.");
            }

            DateTime currentDateTime = DateTime.UtcNow;
            if (dto.StartTime < currentDateTime)
            {
                return BadRequest("Appointment time cannot be in the past.");
            }


            appointment.Duration = duration;

            _mapper.Map(dto, appointment);

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int doctorId, int appointmentId)
        {
            var doctor = await _context.Doctors.Include(d => d.Appointments).FirstOrDefaultAsync(d => d.Id == doctorId);
            if (doctor == null)
                return NotFound("Doctor not found.");

            var appointment = doctor.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment == null)
                return NotFound("Appointment not found.");

            doctor.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok();
        }














        [Authorize(Roles = "Appointment Scheduler,Admin,Patient")]


        [HttpGet("AvailableTimeSlots")]
        public IActionResult GetAvailableTimeSlots(string selectedDate, int doctorId)
        {
            // Parse the selectedDate string into a DateTime object
            if (!DateTime.TryParse(selectedDate, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Please provide the date in YYYY-MM-DD format.");
            }

            DateTime currentDateTime = DateTime.UtcNow;
            DateTime lastDateAllowed = currentDateTime.AddDays(30);

            if (parsedDate < currentDateTime || parsedDate > lastDateAllowed)
            {
                return BadRequest("Selected date must be within the next 30 days from today.");
            }


            var workingSchedule = _context.WorkingSchedules
        .Include(ws => ws.WorkingDays)
        .FirstOrDefault(ws => ws.DoctorId == doctorId);


            if (workingSchedule == null)
            {
                return BadRequest("Working schedule not found for the specified doctor.");
            }

            var parsedDayOfWeek = parsedDate.DayOfWeek;
            var isWorkingDay = workingSchedule.WorkingDays.Any(wd => wd.DayOfWeek == parsedDayOfWeek);

            if (!isWorkingDay)
            {
                return BadRequest("Working schedule not found for the specified day.");
            }

            TimeSpan interval = new TimeSpan(0, 30, 0); // Interval of 30 minutes
            List<DateTime> timeSlots = GenerateTimeSlots(parsedDate, workingSchedule.StartTime, workingSchedule.EndTime, interval);

            var occupiedTimeSlots = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.StartTime.Date == parsedDate.Date)
                .Select(a => a.StartTime.TimeOfDay)
                .ToList();

            timeSlots = timeSlots
                .Where(ts => !occupiedTimeSlots.Contains(ts.TimeOfDay))
                .ToList();

            List<string> formattedTimeSlots = timeSlots.Select(ts => ts.ToString("HH:mm")).ToList();

            return Ok(formattedTimeSlots);
        }


        private List<DateTime> GenerateTimeSlots(DateTime selectedDate, TimeSpan startTime, TimeSpan endTime, TimeSpan interval)
        {
            List<DateTime> timeSlots = new List<DateTime>();
            DateTime currentTimeSlot = selectedDate.Date + startTime;

            while (currentTimeSlot.TimeOfDay <= endTime)
            {
                timeSlots.Add(currentTimeSlot);
                currentTimeSlot = currentTimeSlot.Add(interval);
            }

            return timeSlots;
        }


    }
}
