using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Newtonsoft.Json;

namespace Gasmon
{
    public static class Notifications
    {
        public static async Task GetCurrentMessages(AmazonSQSClient sqsClient, string queueUrl)
        {
            var messageResponse = await sqsClient.ReceiveMessageAsync(queueUrl);
            //If duplicate was a problem could add list to add all messages currently received, filter out that list using 
            //the distinct method and then when clearing out the queue also wipe the contents of the lists.
            var totalReadings = 0;

            foreach (var message in messageResponse.Messages)
            {
                totalReadings += 1;
                var messageBody = JsonConvert.DeserializeObject<MessageBody>(message.Body);
                if (messageBody?.Message != null)
                {
                    var response = JsonConvert.DeserializeObject<MessageFromBody>(messageBody.Message);
                }
            }

            var averageOfTotalReadings  = GetAverageOfReadings(totalReadings);
        }

        private static int GetAverageOfReadings(int totalReadings)
        {
            return totalReadings / 60;
        }
    }
}