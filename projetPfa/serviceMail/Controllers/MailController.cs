using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using serviceMail.DTO;
using serviceMail.Interfaces;

namespace serviceMail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _mailService;
        public MailController(IEmailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            await _mailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return Ok(new { message = "Email sent successfully" });
        }
    }
}
