using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonS3Microservice.Dtos
{
    public class S3Request
    {
        public IFormFile File { get; set; }
    }
}
