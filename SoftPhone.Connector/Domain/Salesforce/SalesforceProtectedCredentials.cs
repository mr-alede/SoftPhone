using SoftPhone.Core.Domain.Salesforce;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SoftPhone.Connector.Domain.Salesforce
{
	[Serializable]
	public class SalesforceProtectedCredentials
	{
		public string Login { get; protected set; }
		public byte[] Password { get; protected set; }
		public byte[] SecurityToken { get; protected set; }

		public SalesforceProtectedCredentials(SalesforceCredentials credentials)
		{
			Login = credentials.Login;

			var password = Encoding.ASCII.GetBytes(credentials.Password);
			Password = ProtectedData.Protect(password, null, DataProtectionScope.CurrentUser);

			var token = Encoding.ASCII.GetBytes(credentials.SecurityToken);
			SecurityToken = ProtectedData.Protect(token, null, DataProtectionScope.CurrentUser);
		}

		public SalesforceCredentials ExtractCredentials()
		{
			var credentials = new SalesforceCredentials();
			credentials.Login = Login;

			var password = ProtectedData.Unprotect(Password, null, DataProtectionScope.CurrentUser);
			credentials.Password = Encoding.ASCII.GetString(password);

			var token = ProtectedData.Unprotect(SecurityToken, null, DataProtectionScope.CurrentUser);
			credentials.SecurityToken = Encoding.ASCII.GetString(token);

			return credentials;
		}
	}
}
