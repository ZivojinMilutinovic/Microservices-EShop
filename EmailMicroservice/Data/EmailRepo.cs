using EmailMicroservice.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMicroservice.Data
{
    public class EmailRepo:IEmailRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmailRepo> _logger;

        public EmailRepo(AppDbContext context,ILogger<EmailRepo> logger)
        {
            _context = context;
            _logger = logger;
        }
      public async  Task<EmailUser> AddEmailUser(EmailUser emailUser)
        {
            try
            {
                _logger.LogInformation("EmailRepo--> Calling method for ading a user");
                var user = await _context.EmailUsers.AddAsync(emailUser);
                
                await _context.SaveChangesAsync();
                _logger.LogInformation("EmailRepo--> User successfuly saved");
                return user.Entity;
            }catch(Exception e)
            {
                Console.WriteLine($"--> Error message happened {e.Message}");
                _logger.LogInformation($"EmailRepo--> Error message happened {e.Message}");
                return new EmailUser();
            } 
        }

        public async Task<bool> SaveChanges()
        {
            _logger.LogInformation("EmailRepo--> Saving scahnges");
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
