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

namespace Hospital_FinalP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IEmailService _emailSender;
        private readonly AppDbContext _context;

        public EmailController(IEmailService emailSender, AppDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }




        [HttpGet]
        public async Task<IActionResult> GetAllEmails()
        {
            try
            {
                var emails = await _context.Emails.ToListAsync();
                return Ok(emails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        

        [HttpPost]
        public async Task<IActionResult> SendEmailToPatient([FromBody] EmailPostDto emailDto)
        {
            var fromDisplayName= "REY Hospital";

            try
            {
                await _emailSender.SendEmailAsync(fromDisplayName, emailDto.To, emailDto.Subject, emailDto.Body, emailDto.From);

                var email = new Email
                {
                    From = emailDto.From,
                    To = emailDto.To,
                    Subject = emailDto.Subject,
                    Body = emailDto.Body,
                };

                _context.Emails.Add(email);
                await _context.SaveChangesAsync();
                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
