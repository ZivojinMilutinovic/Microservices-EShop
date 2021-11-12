using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AmazonS3Microservice.Dtos;
using AmazonS3Microservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AmazonS3Microservice.Data
{
    public class AmazonS3Repo : IAmazonS3Repo
    {
        private readonly IAmazonS3 _client;
        private readonly AppDbContext _context;
        private readonly ILogger<AmazonS3Repo> _logger;

        public AmazonS3Repo(IAmazonS3 client, AppDbContext context, ILogger<AmazonS3Repo> logger)
        {
            _client = client;
            _context = context;
            _logger = logger;
        }
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                _logger.LogInformation("AmazonS3Repo--> Starting method for creating a bucket");
                Console.WriteLine(await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName));
                if (!await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName))
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                    };
                    var response = await _client.PutBucketAsync(putBucketRequest);
                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
                _logger.LogInformation("AmazonS3Repo--> Finishing method for creating a bucket");

            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError("AmazonS3Repo--> Error happened when creating a bucket");
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                _logger.LogError("AmazonS3Repo--> Error happened when creating a bucket");
                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }
            _logger.LogError("AmazonS3Repo--> Error happened when creating a bucket");
            return new S3Response
            {
                Message = "Something went wrong",
                Status = HttpStatusCode.InternalServerError
            };
        }

        public async Task<bool> deleteFileAsync(string bucketName, int productId)
        {
            try
            {
                _logger.LogInformation("AmazonS3Repo--> Starting method for deleting a file ");
                var productPictures = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
               productPictures.ForEach(e =>
                {
                    _client.DeleteObjectAsync(new DeleteObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = e.PictureUrl
                    });
                });
                _context.RemoveRange(productPictures);
                await _context.SaveChangesAsync();
                _logger.LogInformation("AmazonS3Repo--> File was successfuly deleted");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AmazonS3Repo--> An error happened when deleteing a file {e.Message}");
                return false;
            }
        }

        public async Task<Stream> GetObjectFromS3Async(string bucketName, string FileName)
        {
            try
            {
                _logger.LogInformation($"AmazonS3Repo--> Starting a method for getting an object from a S3 bucket");
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = FileName
                };
                using (var response = await _client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                {
                    _logger.LogInformation($"AmazonS3Repo--> Finsihing a method for getting an object from a S3 bucket");
                    return responseStream;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"AmazonS3Repo--> An error happened when gettingan object from a bucket {e.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<ProductImage>> GetPicturesForProduct(int id)
        {
            _logger.LogInformation($"AmazonS3Repo--> Returning all pictures for a product");
            return await _context.ProductImages.Where(x => x.ProductId == id).ToListAsync();
        }

        public async Task<bool> uploadFileAsync(string bucketName, IFormFile file, string productId)
        {
            if (file is null)
            {
                _logger.LogWarning($"AmazonS3Repo--> The file to be posted was null...");
                return false;
            }
               
            try
            {
                _logger.LogInformation($"AmazonS3Repo--> Starting file upload");
                string fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = fileName,
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransfer = new TransferUtility(_client);
                    await fileTransfer.UploadAsync(uploadRequest);
                    await _context.ProductImages.AddAsync(new ProductImage()
                    {
                        ProductId=Int32.Parse(productId),
                        PictureUrl=fileName
                    });
                    await _context.SaveChangesAsync();
                }
                _logger.LogInformation($"AmazonS3Repo--> Finishing file upload");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AmazonS3Repo--> An error happened when uploading a File {e.Message}");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> uploadMultipleFilesAsync(string bucketName, List<IFormFile> files, string productId)
        {
            _logger.LogInformation($"AmazonS3Repo--> Starting multiple file upload");
            bool result = true;
            for (int i = 0; i < files.Count; i++)
            {
                result = await uploadFileAsync(bucketName, files[i], productId);
            }
            _logger.LogInformation($"AmazonS3Repo--> Finishing multiple file upload");
            return result;
        }
    }
}
