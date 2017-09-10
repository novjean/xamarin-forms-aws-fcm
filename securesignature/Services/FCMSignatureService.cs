using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using securesignature.Models;

namespace securesignature.Services
{
    public class FCMSignatureService
    {
        private HttpClient _client = new HttpClient();

        internal SignatureRequest parseData(string value)
        {
			try
			{
				return JsonConvert.DeserializeObject<SignatureRequest>(value);
			}
			catch
			{
                return new SignatureRequest();
			}
        }

        //Pushing the token along with Device ID to Encore DB
        internal async Task<bool> updateTokenToDBAsync(string id, string regId)
        {

            StringContent query = new StringContent("");

            query.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri uri = new Uri($"{getPostUrl()}{id}?regId={regId}");

            System.Diagnostics.Debug.WriteLine($"Uri Absolute Uri= {uri.AbsoluteUri}");

			var result = await _client.PostAsync(uri, query);
			
            System.Diagnostics.Debug.WriteLine($"Result= {result.IsSuccessStatusCode} , {result.ToString()} , {result.Headers.ToString()}");

			return result.IsSuccessStatusCode;

        }

        private string getPostUrl()
        {
            return new Api().api + "Signature/RegisterDevice/";
        }

    }
}
