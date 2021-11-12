using EmailMicroservice.Data;
using EmailMicroservice.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailConfiguration> _logger;

        public EmailController(IEmailSender emailSender,ILogger<EmailConfiguration> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpPost]
        public IActionResult SendEmail([FromBody] MessageDto message )
        {
            _logger.LogInformation("EmailController--> SendEmail method called");
            _emailSender.SendEmailAsync(new Message(message.To,message.Subject,message.Content));
            return Ok();
        }
    }
}
