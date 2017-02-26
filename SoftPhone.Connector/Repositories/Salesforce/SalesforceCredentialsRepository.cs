using SoftPhone.Core.Repositories.Salesforce;
using SoftPhone.Core.Domain.Salesforce;
using SoftPhone.Connector.Domain.Salesforce;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SoftPhone.Connector.Repositories.Salesforce
{
	public class SalesforceCredentialsRepository : ISalesforceCredentialsRepository
	{
		public SalesforceCredentials ReadCredentials()
		{
			try
			{
				var isoFile = IsolatedStorageFile.GetUserStoreForDomain();
				using (var isoStream = new IsolatedStorageFileStream("SalesforceCredentials", FileMode.Open, FileAccess.Read, isoFile))
				{
					var formatter = new BinaryFormatter();
					var protectedCreds = (SalesforceProtectedCredentials)formatter.Deserialize(isoStream);

					return protectedCreds.ExtractCredentials();
				}
			}
			catch (System.Exception)
			{
				return new SalesforceCredentials();
			}
		}

		public void SaveCredentials(SalesforceCredentials credentials)
		{
			var protectedCreds = new SalesforceProtectedCredentials(credentials);

			var isoFile = IsolatedStorageFile.GetUserStoreForDomain();
			using (var isoStream = new IsolatedStorageFileStream("SalesforceCredentials", FileMode.OpenOrCreate, FileAccess.Write, isoFile))
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(isoStream, protectedCreds);
			}
		}
	}
}
