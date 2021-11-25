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
            return sqsClient.CreateQueueAsync(new CreateQueueRequest
            {
                QueueName = "beskra1-locations-queue"
            }).Result;
            
        }
    }
}