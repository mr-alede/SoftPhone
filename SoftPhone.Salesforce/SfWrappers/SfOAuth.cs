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
		string loginEndpoint = "https://login.salesforce.com/services/oauth2/token";
		//string userName = ConfigurationManager.AppSettings["UserName"];
		//string password = ConfigurationManager.AppSettings["Password"] + ConfigurationManager.AppSettings["SecurityToken"];
		//string clientId = ConfigurationManager.AppSettings["ClientId"];
		//string clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

		public Dictionary<string, string> Login(SalesforceCredentials credentials)
		{
			String jsonResponse;

			using (var client = new HttpClient())
			{
				var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", SalesforceSettings.ConsumerKey},
                    {"client_secret", SalesforceSettings.ConsumerSecret},
                    {"username", credentials.Login},
                    {"password", credentials.Password + credentials.SecurityToken}
                });

				request.Headers.Add("X-PrettyPrint", "1");

				var response = client.PostAsync(loginEndpoint, request).Result;
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
