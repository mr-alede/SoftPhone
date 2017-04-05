using SoftPhone.Core.Core;

namespace SoftPhone.Core.Events.Salesforce
{
	public class SalesforceOutcomingCallEvent : IDomainEvent
	{
		public string Id { get; private set; }
		public string SelfUri { get; private set; }
		public string CaleeUri { get; private set; }

		public SalesforceOutcomingCallEvent(string id, string selfUri, string caleeUri)
		{
			Id = id;

			SelfUri = selfUri;
			CaleeUri = caleeUri;
		}
	}

}


