
using SoftPhone.Core.Core;

namespace SoftPhone.Core.Domain.Conversations
{
	public class ConversationEvent: IDomainEvent
	{
		public AppConversation Conversation { get; private set; }
		public ConversationEvent(AppConversation conversation)
		{
			Conversation = conversation;
		}
	}
}
