using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Services.Salesforce;

namespace SoftPhone.Salesforce.EventHandlers.Lync
{
	public class ConversationEventHandler : IDomainEventHandler<ConversationEvent>
	{
		private readonly ISfApiService _sfApi;

		private static AppConversation _lastInserted = null;

		public ConversationEventHandler(ISfApiService sfApi)
		{
			_sfApi = sfApi;
		}

		public async void Handle(ConversationEvent evt)
		{
			if (!evt.Conversation.IsExternalCall)
				return;

			if (evt.Conversation.Status != ConversationStatus.Finished)
			{
				_lastInserted = await _sfApi.Insert(evt.Conversation);
			}
			else
			{
				evt.Conversation.SalesforceId = _lastInserted.SalesforceId;

				await _sfApi.Update(evt.Conversation);

				_lastInserted = null;
			}
		}
	}
}
