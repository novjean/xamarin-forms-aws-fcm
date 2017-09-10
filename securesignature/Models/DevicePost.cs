using System;
namespace securesignature.Models
{
    public class DevicePost
    {
        public string deviceIdentifier { get; set; }

        public DevicePost(string identifier)
        {
            this.deviceIdentifier = identifier;
        }
    }
}
