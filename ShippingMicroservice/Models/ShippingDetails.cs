using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingMicroservice.Models
{
    public class ShippingDetails
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Address { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double TotalSum { get; set; }

        public int CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }
    }
}
