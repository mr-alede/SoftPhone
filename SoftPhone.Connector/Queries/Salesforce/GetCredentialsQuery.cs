using SoftPhone.Core.Queries.Salesforce;
using SoftPhone.Core.Domain.Salesforce;
using System.IO;
using SoftPhone.Connector.Domain.Salesforce;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.IsolatedStorage;

namespace SoftPhone.Connector.Queries.Salesforce
{
	public class GetCredentialsQuery : IGetCredentialsQuery
	{
		public SalesforceCredentials Execute()
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
	}
}
