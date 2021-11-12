using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShippingMicroservice.Dtos;
using ShippingMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingMicroservice.Data
{
    public class ShippingRepo : IShippingRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ShippingRepo> _logger;

        public ShippingRepo(AppDbContext context,ILogger<ShippingRepo> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> CreateShippingDetails(CreateShippingDetailsDto dto)
        {
            _logger.LogInformation("ShippingRepo-->Calling metod for creating shipping details from repo");
            try
            {
                var creditCard = new CreditCard()
                {
                    CardHolder = dto.CardHolder,
                    CardNumber = dto.CardNumber,
                    CCV = dto.CCV,
                    ExpirationDateMonth = dto.ExpirationDateMonth,
                    ExpirationDateYear = dto.ExpirationDateYear,
                    UserId = dto.UserId
                };
                var existingCreditCard = await _context.CreditCard.FirstOrDefaultAsync(x => x.UserId == creditCard.UserId);
                if (existingCreditCard == null)
                {
                    _logger.LogInformation("ShippingRepo-->Existing card was null");
                    await _context.CreditCard.AddAsync(creditCard);
                    await _context.SaveChangesAsync();
                    existingCreditCard = creditCard;
                }else
                {
                    _logger.LogInformation("ShippingRepo-->Existing card was already in database");
                }
                var shippingDetails = new ShippingDetails()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Address = dto.Address,
                    CreditCard = existingCreditCard,
                    CreditCardId = existingCreditCard.Id,
                    UserId=dto.UserId
                };
                await _context.ShippingDetails.AddAsync(shippingDetails);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ShippingRepo-->An error occured while trying to save shipping details ${e.Message}");
                return false;
            }
        }
        public async Task<List<ShippingDetails>>GetShippingDetailsForUser(int userId)
        {
            _logger.LogInformation("ShippingRepo-->Calling method for getting shipping details for user");
            var result =await _context.ShippingDetails.Include(x => x.CreditCard).Where(x => x.UserId == userId).ToListAsync();
            return result;
        }
    }
}
