using AmazonS3Microservice.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AmazonS3Microservice.Data
{
    public interface IAmazonS3Repo
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
        Task<bool> uploadFileAsync(string bucketName, IFormFile file, string productId);
        Task<bool> uploadMultipleFilesAsync(string bucketName, List<IFormFile> files, string productId);
        Task<Stream> GetObjectFromS3Async(string bucketName, string FileName);
        Task<bool> deleteFileAsync(string bucketName, int productId);
    }
}
