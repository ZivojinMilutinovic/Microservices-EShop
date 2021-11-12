using ShippingMicroservice.Dtos;
using ShippingMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingMicroservice.Data
{
    public interface IShippingRepo
    {
        Task<bool> CreateShippingDetails(CreateShippingDetailsDto dto);
        Task<List<ShippingDetails>> GetShippingDetailsForUser(int userId);
    }
}
