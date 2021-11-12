using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMicroservice.Dtos
{
    public class MessageDto
    {
        public IEnumerable<string> To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
