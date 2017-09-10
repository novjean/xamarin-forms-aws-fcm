using System;
using System.Net.Http;

namespace securesignature.Models
{
    public class DeviceToken
    {
        public string deviceToken { get; set; }
        public string expires { get; set; }
        public string host { get; set; }

        public DeviceToken()
        {
            this.deviceToken = "";
            this.expires = "";
            this.host = "";
        }
    }
}
