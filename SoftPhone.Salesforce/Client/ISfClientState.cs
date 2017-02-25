using SoftPhone.Core.Domain.Conversations;

namespace SoftPhone.Salesforce.Client
{
	public interface ISfClientState
	{
		void Connect();
		void Disconnect();
		void Push(Conversation conversation);
	}
}
