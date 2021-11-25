using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gasmon
{
    class Program
    {
        private static AWSCredentials Credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("accessKey"),
            Environment.GetEnvironmentVariable("secretKey"));
        static async Task Main(string[] args)
        {
            await CreateSqsQueue();
            await GetS3Information();
        }

        private static async Task GetS3Information()
        {
            var s3Client = new AmazonS3Client(Credentials, RegionEndpoint.EUWest1);
            const string bucketName = "eventprocessing-swapprentices20-locationss3bucket-qu0txg2hhzj2";

            var objectResponse = await s3Client.GetObjectAsync(bucketName, "locations.json");

            using var objectReader = new StreamReader(objectResponse.ResponseStream);
            var jsonResponse = await objectReader.ReadToEndAsync();
            
            JsonConvert.DeserializeObject<List<Location>>(jsonResponse);
        }

        private static async Task CreateSqsQueue()
        {
            var topicArn = "arn:aws:sns:eu-west-1:552908040772:EventProcessing-SWApprentices2021-snsTopicSensorDataPart1-DF8ZTFFN636Z";
            var sqsClient = new AmazonSQSClient(Credentials, RegionEndpoint.EUWest1);
            var snsClient = new AmazonSimpleNotificationServiceClient(Credentials, RegionEndpoint.EUWest1);
            
            var createQueueResponse = sqsClient.CreateQueueAsync(new CreateQueueRequest
            {
                QueueName = "beskra-locations-queue"
            }).Result;
            var queueUrl = createQueueResponse.QueueUrl;
            await snsClient.SubscribeQueueAsync(topicArn, sqsClient, queueUrl);
            
            
            var messageResponse = await sqsClient.ReceiveMessageAsync(queueUrl);
            //If duplicate was a problem could add list to add all messages currently received, filter out that list using 
            //the distinct method and then when clearing out the queue also wipe the contents of the lists.


            foreach (var message in messageResponse.Messages)
            {
                var messageBody = JsonConvert.DeserializeObject<MessageBody>(message.Body);
                if (messageBody?.Message != null)
                {
                    JsonConvert.DeserializeObject<MessageFromBody>(messageBody.Message);
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
            await sqsClient.DeleteQueueAsync(queueUrl);
        }
    }
}