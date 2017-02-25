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

		public SalesforceProtectedCredentials(SalesforceCredentials credentials)
		{
			Login = credentials.Login;
			var password = Encoding.ASCII.GetBytes(credentials.Password);
			Password = ProtectedData.Protect(password, null, DataProtectionScope.CurrentUser);
		}

		public SalesforceCredentials ExtractCredentials()
		{
			var credentials = new SalesforceCredentials();
			credentials.Login = Login;

			var password = ProtectedData.Unprotect(Password, null, DataProtectionScope.CurrentUser);
			credentials.Password = Encoding.ASCII.GetString(password);

			return credentials;
		}
	}
}
