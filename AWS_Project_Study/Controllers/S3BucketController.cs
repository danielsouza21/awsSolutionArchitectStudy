using Amazon.S3;
using Amazon.S3.Model;
using AWS_Project_Study.Models.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace AWS_Project_Study.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class S3BucketController : ControllerBase
    {
        private readonly ILogger<S3BucketController> _logger;

        private readonly IAmazonS3 _s3Client;
        private readonly S3BucketConfigs _s3BucketConfigs;

        public S3BucketController(ILogger<S3BucketController> logger, IAmazonS3 s3Client, S3BucketConfigs s3BucketConfigs)
        {
            _logger = logger;
            _s3Client = s3Client;
            _s3BucketConfigs = s3BucketConfigs;
        }

        [HttpPost("update-excel")]
        public async Task<IActionResult> UpdateExcelFile(IFormFile excelFile, string origin)
        {
            var key = Path.GetFileName(excelFile.FileName);

            // Validate file name
            if (!_s3BucketConfigs.AllowedFiles.Contains(key))
            {
                return BadRequest("File name is not allowed.");
            }

            using var newMemoryStream = new MemoryStream();
            await excelFile.CopyToAsync(newMemoryStream);

            var putRequest = new PutObjectRequest
            {
                BucketName = _s3BucketConfigs.BucketName,
                Key = key,
                InputStream = newMemoryStream
            };
            putRequest.Metadata.Add("x-amz-meta-origin", origin);

            var response = await _s3Client.PutObjectAsync(putRequest);
            return Ok(new { response.VersionId });
        }
    }
}