using System;

namespace securesignature.Models
{
    public class AWSCredentialModel
    {
		public string AccessKey { get; set; }
		public string SecretKey { get; set; }
		public Amazon.RegionEndpoint Region { get; set; }
		public string PlatformARN { get; set; }
    }
}
