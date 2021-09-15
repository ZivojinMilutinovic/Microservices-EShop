using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Dtos
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
