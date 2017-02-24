using SoftPhone.Core.DomainEvents;

namespace SoftPhone.Core.Conversations
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
