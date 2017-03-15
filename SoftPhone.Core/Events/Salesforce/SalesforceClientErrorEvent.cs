using SoftPhone.Core.Core;

namespace SoftPhone.Core.Events.Salesforce
{
	public class SalesforceClientErrorEvent : ErrorEventBase
	{
		public SalesforceClientErrorEvent(string message):base(message)
		{
		}
	}

}

