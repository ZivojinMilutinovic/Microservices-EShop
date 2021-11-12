using AmazonS3Microservice.Data;
using AmazonS3Microservice.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AmazonS3Controller> _logger;

        public AmazonS3Controller(IAmazonS3Repo amazonS3Repo,ILogger<AmazonS3Controller> logger)
        {
            _amazonS3Repo = amazonS3Repo;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string bucketName)
        {
            _logger.LogInformation("AmzonS3Controller--> Starting Post method for creating a bucket");
            var response = await _amazonS3Repo.CreateBucketAsync(bucketName);
            _logger.LogInformation("AmzonS3Controller--> Finishing Post method for creating a bucket");
            return response.Status == HttpStatusCode.OK ? (IActionResult)Ok(response) : BadRequest(response);
        }
        [HttpPost]
        [RequestSizeLimit(40000000)]
        [Route("{bucketName}/{productId}")]
        public async Task<IActionResult> UploadFile([FromRoute] string bucketName,
          [FromRoute] string productId, [FromForm] S3Request request)
        {
            _logger.LogInformation("AmzonS3Controller--> Starting Uploding File in controller");
            if (request.File is null)
            {
                _logger.LogWarning("AmzonS3Controller--> Request file that was send was null... Exiting buy controller");
                return BadRequest("File can not be empty");
            }
            bool result = await _amazonS3Repo.uploadFileAsync(bucketName, request.File, productId);
            _logger.LogWarning("--> Ending calling UplodaFile controller");
            return result ? (IActionResult)Ok() : BadRequest();

        }
        [HttpPost]
        [RequestSizeLimit(40000000)]
        [Route("{bucketName}/multi/{productId}")]
        public async Task<IActionResult> UploadMultiFiles([FromRoute] string bucketName,
          [FromRoute] string productId, List<IFormFile> files)
        {
            _logger.LogInformation("AmzonS3Controller--> Starting method for uploading multiple files");
            bool result = await _amazonS3Repo.uploadMultipleFilesAsync(bucketName, files, productId);
            _logger.LogInformation("AmzonS3Controller--> Finishing method for uploading multiple files");
            return result ? (IActionResult)Ok() : BadRequest();

        }
        [HttpGet("{bucketName}/{fileName}")]
        public async Task<IActionResult> GetObjectFromS3Async([FromRoute] string bucketName, [FromRoute] string fileName)
        {
            _logger.LogInformation("AmzonS3Controller--> Starting method for getting an object from S3 bucket");
            var stream = await _amazonS3Repo.GetObjectFromS3Async(bucketName, fileName);
            if (stream != null)
            {
                _logger.LogInformation("AmzonS3Controller--> Object was succsessfuly found");
                return File(stream, "application/octet-stream");
            }
            _logger.LogInformation("AmzonS3Controller-->Object was not succsessfuly found");
            return NotFound();
        }
        [HttpGet]
        [Route("productPictures/{id}")]
        public async Task<IActionResult> GetProductPictures([FromRoute] int id)
        {
            _logger.LogInformation("AmzonS3Controller--> Starting method for getting pictures for product");
            var productPictures = await _amazonS3Repo.GetPicturesForProduct(id);
            if (productPictures is null)
            {
                _logger.LogInformation("AmzonS3Controller--> Pictures for product were not found");
                return NotFound();
            }
            _logger.LogInformation("AmzonS3Controller--> Finishing method for returning pictures for product");
            return Ok(productPictures);
        }
        [HttpDelete("{bucketName}/{productId}")]
        public async Task<IActionResult> DeleteFiles([FromRoute] string bucketName, [FromRoute] int productId)
        {
            _logger.LogInformation("AmzonS3Controller--> Starting method for deleting files for product");
            var result = await _amazonS3Repo.deleteFileAsync(bucketName, productId);
            if (result)
            {
                _logger.LogInformation("AmzonS3Controller--> File was successfuly found and deleted");
                return Ok();
            }
            _logger.LogInformation("AmzonS3Controller--> File was not successfully found");
            return NotFound();
        }
    }
}
