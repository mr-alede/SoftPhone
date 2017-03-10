using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Salesforce.Client
{
	internal class DisconnectedSfClient : SfClientStateBase
	{

		public DisconnectedSfClient()
		{
		}

		public override void Connect(SalesforceCredentials credentials)
		{
			SfClient.State = new ConnectingSfClient(credentials);
		}

		public override void Disconnect()
		{
		}
	}
}
