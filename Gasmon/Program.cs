using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Newtonsoft.Json;

namespace Gasmon
{
    static class Program
    {
        private static readonly AWSCredentials Credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("accessKey"),
            Environment.GetEnvironmentVariable("secretKey"));

        private static readonly string topicArn = "arn:aws:sns:eu-west-1:552908040772:EventProcessing-SWApprentices2021-snsTopicSensorDataPart1-DF8ZTFFN636Z";

        private static async Task Main(string[] args)
        {
            await GetS3Information();
            await UpdateQueue();
        }

        private static async Task GetS3Information()
        {
            var s3Client = Connections.CreateS3Connection();
            const string bucketName = "eventprocessing-swapprentices20-locationss3bucket-qu0txg2hhzj2";

            var objectResponse = await s3Client.GetObjectAsync(bucketName, "locations.json");

            using var objectReader = new StreamReader(objectResponse.ResponseStream);
            var jsonResponse = await objectReader.ReadToEndAsync();
            
            JsonConvert.DeserializeObject<List<Location>>(jsonResponse);
        }

  

        private static async Task UpdateQueue()
        {
            
            var sqsClient = Connections.CreateSqsConnection();
            var snsClient = Connections.CreateSnsConnection();

            var createQueueResponse = Queue.CreateQueue(sqsClient);
            var queueUrl = createQueueResponse.QueueUrl;
            await snsClient.SubscribeQueueAsync(topicArn, sqsClient, queueUrl);
            
            
            await Notifications.GetCurrentMessages(sqsClient, queueUrl);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            await Queue.DeleteCurrentQueue(sqsClient, queueUrl);
        }

    }
}