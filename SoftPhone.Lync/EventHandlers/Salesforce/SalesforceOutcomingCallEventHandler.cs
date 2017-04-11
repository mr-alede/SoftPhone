using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Extensibility;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Events.Lync;
using SoftPhone.Core.Events.Salesforce;

namespace SoftPhone.Lync.EventHandlers.Salesforce
{
	public class SalesforceOutcomingCallEventHandler : IDomainEventHandler<SalesforceOutcomingCallEvent>
	{
		private readonly IAppLogger _logger;

		public SalesforceOutcomingCallEventHandler(IAppLogger logger)
		{
			_logger = logger;
		}


		public void Handle(SalesforceOutcomingCallEvent evt)
		{
			_logger.Debug(string.Format("Outbound call detected: {0}", evt.Id));


			var self = LyncClient.GetClient().Self;

			if(self != null)
			{
				string selfSip = AppConversation.Normalize(self.Contact.Uri);

				if(evt.SelfUri == selfSip)
				{
					var appConversation = new AppConversation("", ConversationStatus.OutboundSFDC)
					{
						Self = new Core.Domain.Conversations.Contact { Uri = evt.SelfUri },
						Other = new Core.Domain.Conversations.Contact { Uri = evt.CaleeUri }
					};
					EventsAggregator.Raise(new LyncClientStartConversationEvent(appConversation));

					string calleeSip = evt.CaleeUri.Contains("@") ? string.Format("sip:{0}", evt.CaleeUri) : string.Format("tel:{0}", evt.CaleeUri);

					LyncClient.GetAutomation().BeginStartConversation(
									AutomationModalities.Audio,
									new string[] { calleeSip },
									null,
									(ar) =>
									{
										try
										{
											ConversationWindow newWindow = LyncClient.GetAutomation().EndStartConversation(ar);
										}
										catch (OperationException oe)
										{
											_logger.Error("Operation exception on start conversation " + oe.Message);
										};
									},
									null);

				}
			}
		}
	}

}
