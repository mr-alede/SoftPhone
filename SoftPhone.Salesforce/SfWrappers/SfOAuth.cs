using Newtonsoft.Json;
using SoftPhone.Core.Domain.Salesforce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.SfWrappers
{
	public class SfOAuth
	{
		public Dictionary<string, string> Login(SalesforceCredentials credentials)
		{
			var settings = credentials.Settings;

			String jsonResponse;

			using (var client = new HttpClient())
			{
				var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", settings.ConsumerKey},
                    {"client_secret", settings.ConsumerSecret},
                    {"username", credentials.Login},
                    {"password", credentials.Password + settings.SecurityToken}
                });

				request.Headers.Add("X-PrettyPrint", "1");

				var response = client.PostAsync(settings.InstanceUrl, request).Result;
				jsonResponse = response.Content.ReadAsStringAsync().Result;
			}

			var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
			return values;
		}

		static SfOAuth()
		{
			// SF requires TLS 1.1 or 1.2
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
		}
	}

}
