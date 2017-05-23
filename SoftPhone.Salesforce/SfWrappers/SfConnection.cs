using Salesforce.Common;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Salesforce.SfWrappers
{
	public class SfConnection
	{
		public string Id { get; private set; }
		public string InstanceUrl { get; private set; }
		public string ApiVersion { get; private set; }
		public string AccessToken { get; private set; }

		public SfConnection()
		{
		}

		public async System.Threading.Tasks.Task Login(SalesforceCredentials credentials)
		{
			using (var auth = new AuthenticationClient())
			{
				var url = credentials.InstanceUrl;// "https://login.salesforce.com/services/oauth2/token";

				await auth.UsernamePasswordAsync(SalesforceSettings.ConsumerKey, SalesforceSettings.ConsumerSecret, credentials.Login, credentials.Password + credentials.SecurityToken, url);

				this.Id = auth.Id;
				this.InstanceUrl = auth.InstanceUrl;
				this.ApiVersion = auth.ApiVersion;
				this.AccessToken = auth.AccessToken;
			}
		}
	}
}
