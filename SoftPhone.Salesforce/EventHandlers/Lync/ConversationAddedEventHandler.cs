using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Salesforce.SfWrappers;

namespace SoftPhone.Salesforce.EventHandlers.Lync
{
	public class ConversationAddedEventHandler : IDomainEventHandler<ConversationAddedEvent>
	{
		private readonly ISfApiService _sfApi;

		public ConversationAddedEventHandler(ISfApiService sfApi)
		{
			_sfApi = sfApi;
		}

		public void Handle(ConversationAddedEvent evt)
		{
			_sfApi.Insert(evt.Conversation, ConversationStatus.Inbound);
		}
	}
}
