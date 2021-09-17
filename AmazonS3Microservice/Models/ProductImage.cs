using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonS3Microservice.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string PictureUrl { get; set; }
    }
}
