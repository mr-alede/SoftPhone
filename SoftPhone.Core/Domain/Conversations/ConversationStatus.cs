namespace SoftPhone.Core.Domain.Conversations
{
	public enum ConversationStatus
	{
		Inbound = 0,
		OutboundSkype = 1,
		OutboundSFDC = 2,
		Unanswered = 3,
		Finished = 4
	}

	public static class ConversationStatusEx
	{
		public static string ToLookupString(this ConversationStatus status)
		{
			if (status == ConversationStatus.OutboundSkype)
				return "Outbound Skype";

			if (status == ConversationStatus.OutboundSFDC)
				return "Outbound SFDC";

			return status.ToString();
		}
	}
}
