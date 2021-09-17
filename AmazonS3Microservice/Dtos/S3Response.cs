using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AmazonS3Microservice.Dtos
{
    public class S3Response
    {
        public string Message { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}
