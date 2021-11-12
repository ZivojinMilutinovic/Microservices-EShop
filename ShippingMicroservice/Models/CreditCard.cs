using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingMicroservice.Models
{
    public class CreditCard
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public string CardNumber { get; set; }

        public string CardHolder { get; set; }

        public int ExpirationDateMonth { get; set; }

        public int ExpirationDateYear { get; set; }

        public int CCV { get; set; }
    }
}
