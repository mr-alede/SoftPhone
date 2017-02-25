using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Salesforce.Client
{
	public static class SfClient
	{
		internal static SfClientStateBase State = new DisconnectedSfClient();

		public static void Init()
		{
			State.Connect(new SalesforceCredentials { Login = "mr.alede1@gmail.com", Password="_passpass123" + "fND1mf1NKH9IKuMfcNEIfICiu" });
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
