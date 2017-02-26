using SoftPhone.Core.Core;

namespace SoftPhone.Core.Events.Salesforce
{
	public class SalesforceClientErrorEvent : IDomainEvent
	{
		public string Message { get; private set; }
		public SalesforceClientErrorEvent(string message)
		{
			Message = message;
		}
	}

}

