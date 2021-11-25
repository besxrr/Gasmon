using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gasmon
{
    public class MessageBody
    {
        
        
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string TopicArn { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string SignatureVersion { get; set; }
        public string Signature { get; set; }
        public string SigningCertUrl { get; set; }
        public string UnsubscribeUrl { get; set; }
    }
}