using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using SoftPhone.Core.Queries.Salesforce;
using SoftPhone.Salesforce.Client;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.CommandHandlers
{
	public class SalesforceConnectCommandHandler : ICommandHandler<SalesforceConnectCommand>
	{
		public void Execute(SalesforceConnectCommand command)
		{
			var credentials = QueryProcessor.GetQuery<IGetCredentialsQuery>().Execute();

			Task.Run(() => {
				//credentials.Password += "fND1mf1NKH9IKuMfcNEIfICiu"; // salesforce security token
				SfClient.Connect(credentials);
			});
		}
	}

}
