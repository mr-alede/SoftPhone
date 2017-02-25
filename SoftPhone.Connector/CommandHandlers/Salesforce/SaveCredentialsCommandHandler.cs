using SoftPhone.Connector.Domain.Salesforce;
using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;

namespace SoftPhone.Connector.CommandHandlers.Salesforce
{
	public class SaveCredentialsCommandHandler : ICommandHandler<SaveCredentialsCommand>
	{
		public void Execute(SaveCredentialsCommand command)
		{
			var protectedCreds = new SalesforceProtectedCredentials(command.Credentials);

			var isoFile = IsolatedStorageFile.GetUserStoreForDomain();
			using (var isoStream = new IsolatedStorageFileStream("SalesforceCredentials", FileMode.OpenOrCreate, FileAccess.Write, isoFile))
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(isoStream, protectedCreds);
			}
		}
	}
}
