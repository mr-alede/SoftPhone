using Cometd.Bayeux;
using Cometd.Bayeux.Client;
using SoftPhone.Core.Core;
using SoftPhone.Core.Events.Salesforce;
using System.Collections.Generic;

namespace SoftPhone.Salesforce.Client
{
	public class SalesforceListener : IMessageListener
	{
		public void onMessage(IClientSessionChannel channel, IMessage message)
		{
			var data = message.DataAsDictionary;

			var sobject = data["sobject"] as Dictionary<string, object>;

			if (sobject != null)
			{
				string id = sobject["Id"] as string;
				string status = sobject["Status__c"] as string;

				if (status == "Outbound SFDC")
				{
					string selfUri = sobject["Email__c"] as string;
					string caleeUri = sobject["Number__c"] as string;

					EventsAggregator.Raise(new SalesforceOutcomingCallEvent(id, selfUri, caleeUri));
				}
			}
		}
	}
}
