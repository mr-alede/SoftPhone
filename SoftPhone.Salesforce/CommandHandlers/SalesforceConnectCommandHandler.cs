using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using SoftPhone.Core.Repositories.Salesforce;
using SoftPhone.Salesforce.Client;

namespace SoftPhone.Salesforce.CommandHandlers
{
	public class SalesforceConnectCommandHandler : ICommandHandler<SalesforceConnectCommand>
	{
		private readonly ISalesforceCredentialsRepository _repo;
		public SalesforceConnectCommandHandler(ISalesforceCredentialsRepository repo)
		{
			_repo = repo;
		}

		public void Execute(SalesforceConnectCommand command)
		{
			var credentials = _repo.ReadCredentials();
			SfClient.Connect(credentials);
		}
	}

}
