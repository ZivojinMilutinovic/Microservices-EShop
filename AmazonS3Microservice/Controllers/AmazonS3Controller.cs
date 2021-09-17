using AmazonS3Microservice.Data;
using AmazonS3Microservice.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AmazonS3Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmazonS3Controller : ControllerBase
    {
        private readonly IAmazonS3Repo _amazonS3Repo;
        public AmazonS3Controller(IAmazonS3Repo amazonS3Repo)
        {
            _amazonS3Repo = amazonS3Repo;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string bucketName)
        {
            var response = await _amazonS3Repo.CreateBucketAsync(bucketName);
            return response.Status == HttpStatusCode.OK ? (IActionResult)Ok(response) : BadRequest(response);
        }
        [HttpPost]
        [RequestSizeLimit(40000000)]
        [Route("{bucketName}/{productId}")]
        public async Task<IActionResult> UploadFile([FromRoute] string bucketName,
          [FromRoute] string productId, [FromForm] S3Request request)
        {
            if (request.File is null)
            {
                return BadRequest("File can not be empty");
            }
            bool result = await _amazonS3Repo.uploadFileAsync(bucketName, request.File, productId);
            return result ? (IActionResult)Ok() : BadRequest();

        }
        [HttpPost]
        [RequestSizeLimit(40000000)]
        [Route("{bucketName}/multi/{productId}")]
        public async Task<IActionResult> UploadMultiFiles([FromRoute] string bucketName,
          [FromRoute] string productId, List<IFormFile> files)
        {

            bool result = await _amazonS3Repo.uploadMultipleFilesAsync(bucketName, files, productId);
            return result ? (IActionResult)Ok() : BadRequest();

        }
        [HttpGet("{bucketName}/{fileName}")]
        public async Task<IActionResult> GetObjectFromS3Async([FromRoute] string bucketName, [FromRoute] string fileName)
        {
            var stream = await _amazonS3Repo.GetObjectFromS3Async(bucketName, fileName);
            if (stream != null)
            {
                return File(stream, "application/octet-stream");
            }
            return NotFound();
        }
        [HttpDelete("{bucketName}/{productId}")]
        public async Task<IActionResult> DeleteFiles([FromRoute] string bucketName, [FromRoute] int productId)
        {
            var result = await _amazonS3Repo.deleteFileAsync(bucketName, productId);
            if (result)
                return Ok();
            return NotFound();
        }
    }
}
