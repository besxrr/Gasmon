using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gasmon
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("accessKey"),
                Environment.GetEnvironmentVariable("secretKey"));
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.EUWest1);
            const string bucketName = "eventprocessing-swapprentices20-locationss3bucket-qu0txg2hhzj2";

            var objectResponse = await s3Client.GetObjectAsync(bucketName, "locations.json");

            using var objectReader = new StreamReader(objectResponse.ResponseStream);
            var jsonResponse = await objectReader.ReadToEndAsync();
            JsonConvert.DeserializeObject<List<Location>>(jsonResponse);
        }
    }
}