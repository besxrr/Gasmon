using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Gasmon
{
    public static class Queue
    {
        public static async Task DeleteCurrentQueue(AmazonSQSClient sqsClient, string queueUrl)
        {
            await sqsClient.DeleteQueueAsync(queueUrl);
        }
        
        public static CreateQueueResponse CreateQueue(AmazonSQSClient sqsClient)
        {
            var createQueueResponse = sqsClient.CreateQueueAsync(new CreateQueueRequest
            {
                QueueName = "beskra-locations-queue"
            }).Result;
            return createQueueResponse;
        }
    }
}