using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace Gasmon
{
    public static class Connections
    {
        private static readonly AWSCredentials Credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("accessKey"),
            Environment.GetEnvironmentVariable("secretKey"));
        
        public static AmazonSQSClient CreateSqsConnection()
        {
            return new AmazonSQSClient(Credentials, RegionEndpoint.EUWest1);
        }
        
        public static AmazonSimpleNotificationServiceClient CreateSnsConnection()
        {
            return new AmazonSimpleNotificationServiceClient(Credentials, RegionEndpoint.EUWest1);
        }
        
        public static AmazonS3Client CreateS3Connection()
        {
            return new AmazonS3Client(Credentials, RegionEndpoint.EUWest1);
        }
    }
}