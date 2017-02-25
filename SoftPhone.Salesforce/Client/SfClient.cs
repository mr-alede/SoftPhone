using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Salesforce.Client
{
	public static class SfClient
	{
		internal static SfClientStateBase State = new DisconnectedSfClient();

		public static void Connect(SalesforceCredentials credentials)
		{
			State.Connect(credentials);
		}

		public static void Push(Conversation conversation)
		{
			State.Push(conversation);
		}

		public static void Disconnect()
		{
			State.Disconnect();
		}
	}
}
