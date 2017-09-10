using System;
using System.Collections.Generic;

//Model for information sent from Encore on request for the signature
namespace securesignature.Models
{
    public class SignatureRequest
    {
        public List<string> Messages { get; set; }
        public float Weight { get; set; }
        public float Price { get; set; }
        public string TicketNumber { get; set; }
        public string RequestId { get; set; }
        public string Action { get; set; }

        public SignatureRequest()
        {
            Messages = new List<string>();
            Weight = -1;
            Price = -1;
            TicketNumber = "";
            Action = "";
            RequestId = Guid.NewGuid().ToString("N");
        }
    }
}