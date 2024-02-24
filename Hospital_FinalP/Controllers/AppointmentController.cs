using AutoMapper;
using Hangfire;
using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.Department;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Migrations;
using Hospital_FinalP.Services.Abstract;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        private readonly IEmailService _emailService;

        public AppointmentController(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments(int? page,int? perPage,string doctorName = null,bool? isActive = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient);



            if (isActive.HasValue)
            {
                if (isActive.Value)
                {
                    query = query.Where(a => a.IsActive && a.StartTime >= DateTime.UtcNow);
                }
                else 
                {
                    query = query.Where(a => !a.IsActive || a.EndTime < DateTime.UtcNow);
                }
            }

            if (!string.IsNullOrEmpty(doctorName))
            {
                query = query.Where(a => a.Doctor.FullName.Contains(doctorName));
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
                .Select(x => _mapper.Map(x, new AppointmentGetDto()))
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { totalCount, appointments });
        }



        [HttpGet("patients/{patientId}")]
        public async Task<IActionResult> GetAppointmentsForPatient(int patientId, [FromQuery(Name = "_page")] int? page, [FromQuery(Name = "_perPage")] int? perPage)
        {
            if (_context.Appointments == null) return NotFound(); 

            IQueryable<Appointment> appointmentsQuery = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.StartTime);

            if (!perPage.HasValue || perPage <= 0)
            {
                var allAppointments = await appointmentsQuery
                    .Select(x => _mapper.Map<AppointmentGetDto>(x))
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(allAppointments);
            }

            if (!page.HasValue || page < 1)
                page = 1;

            int totalAppointments = await appointmentsQuery.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalAppointments / perPage.Value);

            int skip = (page.Value - 1) * perPage.Value;

            var appointments = await appointmentsQuery
                .Skip(skip)
                .Take(perPage.Value)
                .Select(x => _mapper.Map<AppointmentGetDto>(x))
                .AsNoTracking()
                .ToListAsync();
            return Ok(new { appointments, totalAppointments });
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
                    appointmentsQuery = appointmentsQuery.Where(a => !a.IsActive || a.EndTime < DateTime.UtcNow);
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


            DateTime today = DateTime.UtcNow.Date;

            var todaysAppointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.StartTime.Date == today).Select(a => new
                {
                    AppointmentId=a.Id,
                    DoctorFullName=a.Doctor.FullName,
                    DoctorEmail=a.Doctor.DoctorDetail.Email,
                    PatientFullName = a.Patient.FullName,
                    PatientEmail = a.Patient.Email,
                    StartTime = a.StartTime.ToString("dd-MM-yyyy HH:mm:ss"),
                    EndTime = a.EndTime.ToString("dd-MM-yyyy HH:mm:ss"),
                    a.Description,
                                     
                    a.IsActive
                })
        .ToListAsync();




            var appointments = await appointmentsQuery
                .Select(x => _mapper.Map(x, new AppointmentGetDto()))
                .AsNoTracking()
                .ToListAsync();



            return Ok(new { appointments, totalCount, todaysAppointments });
        }

        //[HttpGet("{doctorId}/appointments/today")]
        //public async Task<IActionResult> GetTodaysAppointmentsForDoctor(int doctorId)
        //{
        //    DateTime today = DateTime.UtcNow.Date;

        //    var appointments = await _context.Appointments
        //        .Include(a => a.Doctor)
        //        .Include(a => a.Patient)
        //        .Where(a => a.DoctorId == doctorId && a.StartTime.Date == today)
        //        .ToListAsync();

        //    return Ok(appointments);
        //}





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





        //[Authorize(Roles = "Patient, Scheduler")]
        [HttpPost]
        public async Task<IActionResult> ScheduleAppointment([FromBody] AppointmentPostDto dto)
        {
            //if (dto.StartTime.Date == DateTime.Today)
            //{
            //    return BadRequest("Booking for today is not allowed.");
            //}


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



            //var existingAppointments = await _context.Appointments
            //                 .Where(a => a.DoctorId == dto.DoctorId && a.StartTime.Date == dto.StartTime.Date)
            //                 .ToListAsync();

            //int bookedAppointmentsCount = existingAppointments.Count;


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

            var sendEmailJobTime = dto.StartTime.Subtract(TimeSpan.FromMinutes(25));
            var senderName = "REY Hospital";
            var to = patient.Email;
            var subject = "Upcoming Appointment";
            var body = $"Your appointment with {doctor.FullName} is scheduled.";
            var from = "REY Hospital <demo1flight@gmail.com>";

            // Schedule email sending 30 minutes before the appointment


            _context.Add(appointment);
            await _context.SaveChangesAsync();
            var patientEmail = patient.Email;

            // Отправка напоминания по электронной почте за час до назначенного времени
            var reminderTime = dto.StartTime.AddMinutes(-20);  // Вычисляем время напоминания
            var currentTime = DateTime.UtcNow;
            var localReminderTime = TimeZoneInfo.ConvertTimeFromUtc(reminderTime, TimeZoneInfo.Local);

            if (localReminderTime > currentTime)
            {
                // Формирование текста сообщения напоминания
                var reminderBody = $"You have an appointment scheduled at {dto.StartTime.ToString()}.";

                // Вызов метода вашей службы отправки электронной почты для отправки напоминания
                await _emailService.SendEmailAsync("REY", patientEmail, "Appointment Reminder", reminderBody, from);
            }



            return Ok(appointment);

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            //var doctor = await _context.Doctors.Include(d => d.Appointments).FirstOrDefaultAsync(d => d.Id == doctorId);
            //if (doctor == null)
            //    return NotFound("Doctor not found.");

            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                return NotFound("Appointment not found.");


            _context.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok();
        }














        //[Authorize(Roles = "Appointment Scheduler,Admin,Patient")]
        //
        //
        //
        //
        //
        //


        //[HttpGet("AvailableTimeSlots")]
        //public IActionResult GetAvailableTimeSlots(string selectedDate, int doctorId)
        //{
        //    if (!DateTime.TryParse(selectedDate, out DateTime parsedDate))
        //        return BadRequest("Invalid date format. Please provide the date in YYYY-MM-DD format.");

        //    DateTime currentDateTime = DateTime.UtcNow;
        //    DateTime lastDateAllowed = currentDateTime.AddDays(30);

        //    if (parsedDate < currentDateTime || parsedDate > lastDateAllowed)
        //        return BadRequest("Selected date must be within the next 30 days from today.");


        //    var workingSchedule = _context.WorkingSchedules
        //.Include(ws => ws.WorkingDays)
        //.FirstOrDefault(ws => ws.DoctorId == doctorId);


        //    if (workingSchedule == null)
        //        return BadRequest("Working schedule not found for the specified doctor.");

        //    var parsedDayOfWeek = parsedDate.DayOfWeek;
        //    var isWorkingDay = workingSchedule.WorkingDays.Any(wd => wd.DayOfWeek == parsedDayOfWeek);

        //    if (!isWorkingDay)
        //        return BadRequest("Working schedule not found for the specified day.");

        //    TimeSpan interval = new TimeSpan(0, 30, 0); // Interval of 30 minutes
        //    List<DateTime> timeSlots = GenerateTimeSlots(parsedDate, workingSchedule.StartTime, workingSchedule.EndTime, interval);

        //    var occupiedTimeSlots = _context.Appointments
        //        .Where(a => a.DoctorId == doctorId && a.StartTime.Date == parsedDate.Date)
        //        .Select(a => a.StartTime.TimeOfDay)
        //        .ToList();

        //    timeSlots = timeSlots
        //        .Where(ts => !occupiedTimeSlots.Contains(ts.TimeOfDay))
        //        .ToList();

        //    List<string> formattedTimeSlots = timeSlots.Select(ts => ts.ToString("HH:mm")).ToList();


        //    return Ok(formattedTimeSlots);
        //}


        //[HttpGet("AvailableTimeSlots")]
        //public IActionResult GetAvailableTimeSlots(string selectedDate, int doctorId)
        //{
        //    DateTime currentDateTime = DateTime.UtcNow;
        //    DateTime today = currentDateTime.Date;

        //    if (!DateTime.TryParse(selectedDate, out DateTime parsedDate))
        //    {
        //        // If selected date is not provided, default to today
        //        parsedDate = today;
        //    }
        //    else
        //    {
        //        // Ensure selected date is within the next 30 days
        //        DateTime lastDateAllowed = currentDateTime.AddDays(30);
        //        if (parsedDate < today || parsedDate > lastDateAllowed)
        //            return BadRequest("Selected date must be within the next 30 days from today.");
        //    }

        //    var workingSchedule = _context.WorkingSchedules
        //        .Include(ws => ws.WorkingDays)
        //        .FirstOrDefault(ws => ws.DoctorId == doctorId);

        //    if (workingSchedule == null)
        //        return BadRequest("Working schedule not found for the specified doctor.");

        //    var parsedDayOfWeek = parsedDate.DayOfWeek;
        //    var isWorkingDay = workingSchedule.WorkingDays.Any(wd => wd.DayOfWeek == parsedDayOfWeek);

        //    if (!isWorkingDay)
        //        return BadRequest("Working schedule not found for the specified day.");

        //    TimeSpan interval = new TimeSpan(0, 30, 0); // Interval of 30 minutes
        //    TimeSpan currentTime = currentDateTime.TimeOfDay;
        //    TimeSpan startTime = workingSchedule.StartTime;

        //    // Adjust start time if it's today
        //    if (parsedDate == today)
        //    {
        //        // Find the nearest half-hour mark after the current time
        //        int minutesToAdd = 30 - (currentDateTime.Minute % 30);
        //        currentTime = currentTime.Add(new TimeSpan(0, minutesToAdd, 0));

        //        if (currentTime > startTime)
        //            startTime = currentTime;
        //    }

        //    List<DateTime> timeSlots = GenerateTimeSlots(parsedDate, startTime, workingSchedule.EndTime, interval);

        //    var occupiedTimeSlots = _context.Appointments
        //        .Where(a => a.DoctorId == doctorId && a.StartTime.Date == parsedDate.Date)
        //        .Select(a => a.StartTime.TimeOfDay)
        //        .ToList();

        //    timeSlots = timeSlots
        //        .Where(ts => !occupiedTimeSlots.Contains(ts.TimeOfDay))
        //        .ToList();

        //    List<string> formattedTimeSlots = timeSlots.Select(ts => ts.ToString("HH:mm")).ToList();

        //    return Ok(formattedTimeSlots);
        //}
        [HttpGet("AvailableTimeSlots")]
        public IActionResult GetAvailableTimeSlots(string selectedDate, int doctorId)
        {
            if (!DateTime.TryParse(selectedDate, out DateTime parsedDate))
                return BadRequest("Invalid date format. Please provide the date in YYYY-MM-DD format.");

            DateTime currentDateTimeUtc = DateTime.UtcNow;
            DateTime currentDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(currentDateTimeUtc, TimeZoneInfo.Local);

            DateTime today = currentDateTimeLocal.Date;
            TimeSpan currentTime = currentDateTimeLocal.TimeOfDay;

            // Calculate last allowed date within the next 30 days
            DateTime lastDateAllowed = currentDateTimeLocal.AddDays(30);

            if (parsedDate < today || parsedDate > lastDateAllowed)
                return BadRequest("Selected date must be within the next 30 days from today.");

            // Adjusting the selected date to today if it's before today
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

            TimeSpan interval = new TimeSpan(0, 30, 0); // Interval of 30 minutes
            List<DateTime> timeSlots = GenerateTimeSlots(parsedDate, workingSchedule.StartTime, workingSchedule.EndTime, interval);

            var occupiedTimeSlots = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.StartTime.Date == parsedDate.Date)
                .Select(a => a.StartTime.TimeOfDay)
                .ToList();

            timeSlots = timeSlots
                .Where(ts => !occupiedTimeSlots.Contains(ts.TimeOfDay))
                .ToList();

            // If it's today, filter out past time slots
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
            TimeSpan lunchStartTime = new TimeSpan(14, 0, 0); // Lunch break start time
            TimeSpan lunchEndTime = new TimeSpan(15, 0, 0); // Lunch break end time

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
