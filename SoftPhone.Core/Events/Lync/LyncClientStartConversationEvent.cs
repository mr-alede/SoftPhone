using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;

namespace SoftPhone.Core.Events.Lync
{
	public class LyncClientStartConversationEvent : IDomainEvent
	{
		public AppConversation Conversation { get; private set; }
		public LyncClientStartConversationEvent(AppConversation conversation)
		{
			Conversation = conversation;
		}
	}

}

