using Hospital_FinalP.Data;
using Hospital_FinalP.DTOs.EmailSender;
using Hospital_FinalP.Entities;
using Hospital_FinalP.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Net.Mail;
using MailBee;
using MailBee.Mime;
using MailBee.SmtpMail;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.Migrations;
using Humanizer;
using AutoMapper;
using Hospital_FinalP.DTOs.Email;
using Hospital_FinalP.Services.Concrete;

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IEmailService _emailSender;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EmailController(IEmailService emailSender, AppDbContext context, IMapper mapper)
        {
            _emailSender = emailSender;
            _context = context;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllEmails(int? page, int? perPage)
        {
            if (_context.Emails == null) return NotFound();

            IQueryable<Email> query = _context.Emails;

            int totalCount = await query.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                query = query.OrderBy(a => a.SentTime).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                query = query.OrderBy(a => a.SentTime);
            }


            var emails = await query
                .Select(x => _mapper.Map(x, new EmailGetDto()))
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { totalCount, emails });

        }


        [HttpGet("{from}")]
        public async Task<IActionResult> GetEmailsBySender(string from, int? page, int? perPage)
        {
            if (_context.Emails == null) return NotFound();

            IQueryable<Email> emailQuery = _context.Emails
                .Where(a => a.From == from && !a.IsDeleted)
                .OrderBy(a => a.SentTime);


            int totalCount = await emailQuery.CountAsync();

            if (page.HasValue && perPage.HasValue)
            {
                int currentPage = page.Value > 0 ? page.Value : 1;
                int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

                int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                currentPage = currentPage > totalPages ? totalPages : currentPage;

                int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

                emailQuery = emailQuery.OrderBy(a => a.SentTime).Skip(skip).Take(itemsPerPage);
            }
            else
            {
                emailQuery = emailQuery.OrderBy(a => a.SentTime);
            }


            var sentEmails = await emailQuery
                .Select(x => _mapper.Map(x, new EmailGetDto()))
                .AsNoTracking()
                .ToListAsync();



            return Ok(new { sentEmails, totalCount });

        }

        [HttpPost]
        public async Task<IActionResult> SendEmailToPatient([FromBody] EmailPostDto emailDto)
        {
            var fromDisplayName= "REY Hospital";

            try
            {
                await _emailSender.SendEmailAsync(fromDisplayName, emailDto.To, emailDto.Subject, emailDto.Body, emailDto.From);

                var email = new Email();
                email = _mapper.Map(emailDto, email);

                _context.Emails.Add(email);
                await _context.SaveChangesAsync();
                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var email = await _context.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }

            // Soft delete
            email.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var email = _context.Emails
                .FirstOrDefault(x => x.Id == id);
            if (email is null) return NotFound("Sent email Not Found");


            _context.Remove(email);
            _context.SaveChanges();
            return Ok();

        }

    }
}
