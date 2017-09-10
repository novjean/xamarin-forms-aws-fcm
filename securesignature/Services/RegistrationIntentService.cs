using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLCrypto;
using securesignature.Models;

namespace securesignature.Services
{
    public class RegistrationIntentService
    {
        GenericRequest genericRequest = new GenericRequest();
        private HttpClient _client = new HttpClient();

        static System.Globalization.DateTimeFormatInfo dti = new System.Globalization.DateTimeFormatInfo();


        //Checking if the Device ID is registered with Encore
        public async Task<DeviceInfo> checkDevice(string UUID)
        {

            //async call to see if the url gets a response
            var response = await _client.GetAsync(getUrl() + UUID);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<DeviceInfo>(content);
                }
                catch 
                {
                    //Log the exception from this service
                    return new DeviceInfo();
                }
            }
            //Returning new object to avoid NullReferenceException
            return new DeviceInfo();
        }

        private string getUrl()
        {
            return new Api().api + "Setup/Key?deviceIdentifier=";
        }

        //Pushing UUID to DB
        internal async Task<bool> PushDeviceIdentifierAsync(string uuid)
        {

            DevicePost deviceIdentifier = new DevicePost(uuid);

            var json = JsonConvert.SerializeObject(deviceIdentifier);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await _client.PostAsync(getPostUrl(), httpContent);

            return result.IsSuccessStatusCode;
        }

        private string getPostUrl()
        {
            return new Api().api + "Setup/Device";
        }

        //Creating the DeviceToken to be sent to DB
        internal async Task<string> BuildDeviceTokenAsync(string privateKey, string deviceId)
        {
            genericRequest.deviceIdentifier = deviceId;
            genericRequest.timestamp = convertDateTimeToFormatedString(DateTime.Now);

            string message = genericRequest.deviceIdentifier + genericRequest.timestamp;

            privateKey = privateKey.Replace("-", "");

            try
            {
                genericRequest.hmac = GenerateHash(message, privateKey);
            }
            catch(Exception)
            {
                //Need to Log failure
                System.Diagnostics.Debug.WriteLine("HMAC failed. ");
            }

            DeviceToken deviceToken = await GetTokenAsync(genericRequest);
            //printDebug("DeviceToken", deviceToken.deviceToken);
            return deviceToken.deviceToken;
        }

        //Receiving the token from DB
        private async Task<DeviceToken> GetTokenAsync(GenericRequest request)
        {
            var json = JsonConvert.SerializeObject(request);

			HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await _client.PostAsync(getTokenUrl(), httpContent );

            if(result.IsSuccessStatusCode){

				var content = await result.Content.ReadAsStringAsync();

				try
				{
					return JsonConvert.DeserializeObject<DeviceToken>(content);
				}
				catch
				{
					//Log the exception from this service
					return new DeviceToken();
				}
            }else{
                return new DeviceToken();
            }
        }

        private string getTokenUrl()
        {
            return new Api().api + "Auth/RequestToken";
        }

        private string convertDateTimeToFormatedString(DateTime dateTime)
        {
            string dateFormat = "yyyy-MM-dd'T'HH:mm:ssZ";
            return dateTime.ToString(dateFormat, dti);
        }

		//HMACSHA256 Hash Generation
		private static string GenerateHash(string input, string key)
		{
            var mac = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha256);
			var keyMaterial = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(key, Encoding.UTF8);
			var cryptoKey = mac.CreateKey(keyMaterial);
			var hash = WinRTCrypto.CryptographicEngine.Sign(cryptoKey, WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(input, Encoding.UTF8));
            return WinRTCrypto.CryptographicBuffer.EncodeToHexString(hash);
		}
    }
}
