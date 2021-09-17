using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AmazonS3Microservice.Dtos;
using AmazonS3Microservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public AmazonS3Repo(IAmazonS3 client,AppDbContext context)
        {
            _client = client;
            _context = context;
        }
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
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

            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {

                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }
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

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<Stream> GetObjectFromS3Async(string bucketName, string FileName)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = FileName
                };
                using (var response = await _client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                {
                    return responseStream;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> uploadFileAsync(string bucketName, IFormFile file, string productId)
        {
            if (file is null) return false;
            try
            {
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
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> uploadMultipleFilesAsync(string bucketName, List<IFormFile> files, string productId)
        {
            bool result = true;
            for (int i = 0; i < files.Count; i++)
            {
                result = await uploadFileAsync(bucketName, files[i], productId);
            }
            return result;
        }
    }
}
