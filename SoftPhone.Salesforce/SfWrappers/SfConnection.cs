using SoftPhone.Salesforce.SalesforceService;
using System;
using System.Net;

namespace SoftPhone.Salesforce.SfWrappers
{
	public class SfConnection
	{
		public string SessionID { get; private set; }
		public string ServerUrl { get; private set; }

		//public SfConnection(string connectionString)
		//{
		//	ForceConnectionStringBuilder connectionBuilder =
		//		new ForceConnectionStringBuilder(connectionString);

		//	Login(connectionBuilder.Username, connectionBuilder.Password, connectionBuilder.Token);
		//}

		public SfConnection(string username, string password, string securityToken)
		{
			Login(username, password, securityToken);
		}

		private void Login(string username, string password, string securityToken)
		{
			using (var service = new SoapClient())
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

				var loginResult = service.login(null, username, String.Concat(password, securityToken));

				this.SessionID = loginResult.sessionId;
				this.ServerUrl = loginResult.serverUrl;
			}
		}
	}
}
