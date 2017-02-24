using SoftPhone.Core.Conversations;
using SoftPhone.Core.DomainEvents;
using System;

namespace SoftPhone.Connector.EventHandlers
{
	public class ConversationAddedEventHandler : IDomainEventHandler<ConversationAddedEvent>
	{
		public void Handle(ConversationAddedEvent args)
		{
			throw new NotImplementedException();
		}
	}
}
