using System;
namespace securesignature.Models
{
    public class GenericRequest
    {
        public string deviceIdentifier { get; set; }
        public string timestamp { get; set; }
        public string hmac { get; set; }

		public GenericRequest()
        {
            this.deviceIdentifier = "";
            this.timestamp = "";
            this.hmac = "";
        }
    }
}
