using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Core.Commands.Salesforce
{
	public class SaveCredentialsCommand:IAppCommand
	{
		public SalesforceCredentials Credentials { get; private set; }

		public SaveCredentialsCommand(SalesforceCredentials credentials)
		{
			Credentials = credentials;
		}
	}
}
