using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Services.Salesforce;

namespace SoftPhone.Salesforce.EventHandlers.Lync
{
	public class ConversationEventHandler : IDomainEventHandler<ConversationEvent>
	{
		private readonly ISfApiService _sfApi;

		public ConversationEventHandler(ISfApiService sfApi)
		{
			_sfApi = sfApi;
		}

		public void Handle(ConversationEvent evt)
		{
			_sfApi.Insert(evt.Conversation);
		}
	}
}
