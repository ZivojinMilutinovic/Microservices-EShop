using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingMicroservice.Dtos
{
    public class CreateShippingDetailsDto
    {
        public string Address { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double TotalSum { get; set; }

        public int UserId { get; set; }

        public string CardNumber { get; set; }

        public string CardHolder { get; set; }

        public int ExpirationDateMonth { get; set; }

        public int ExpirationDateYear { get; set; }

        public int CCV { get; set; }
    }
}
