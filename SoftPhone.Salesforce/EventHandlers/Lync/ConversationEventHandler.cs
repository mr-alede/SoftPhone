using SoftPhone.Core.Core;
using SoftPhone.Core.Domain;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Events.Lync;
using SoftPhone.Core.Services.Salesforce;
using System;

namespace SoftPhone.Salesforce.EventHandlers.Lync
{
	public class ConversationEventHandler : IDomainEventHandler<ConversationEvent>, IDomainEventHandler<LyncClientStartConversationEvent>
	{
		private readonly ISfApiService _sfApi;
		private readonly IAppLogger _logger;

		private static AppConversation _lastInserted = null;

		public ConversationEventHandler(ISfApiService sfApi, IAppLogger logger)
		{
			_sfApi = sfApi;
			_logger = logger;
		}

		public async void Handle(ConversationEvent evt)
		{
			_logger.Debug(string.Format("Skype call detected: {0} -> {1} status: {2}",
				evt.Conversation.Self.Uri,
				evt.Conversation.Other.Uri,
				evt.Conversation.Status.ToLookupString()));

			if (!evt.Conversation.IsExternalCall)
				return;

			if (evt.Conversation.Status != ConversationStatus.Finished && 
				evt.Conversation.Status != ConversationStatus.Unanswered &&
				(_lastInserted == null || _lastInserted.Status != ConversationStatus.OutboundSFDC ))
			{
				_lastInserted = await _sfApi.Insert(evt.Conversation);
			}
			else
			{
				evt.Conversation.SalesforceId = _lastInserted.SalesforceId;

				await _sfApi.Update(evt.Conversation);
			}

			if (evt.Conversation.Status == ConversationStatus.Finished ||
				evt.Conversation.Status == ConversationStatus.Unanswered)
			{
				_lastInserted = null;
			}
		}

		public void Handle(LyncClientStartConversationEvent evt)
		{
			_lastInserted = evt.Conversation;
		}
	}
}
