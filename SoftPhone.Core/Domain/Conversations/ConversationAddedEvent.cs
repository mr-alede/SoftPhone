
using SoftPhone.Core.Core;

namespace SoftPhone.Core.Domain.Conversations
{
	public class ConversationAddedEvent: IDomainEvent
	{
		public Conversation Conversation { get; private set; }
		public ConversationAddedEvent(Conversation conversation)
		{
			Conversation = conversation;
		}
	}
}
