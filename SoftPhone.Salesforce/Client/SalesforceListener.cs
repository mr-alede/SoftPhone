using Cometd.Bayeux;
using Cometd.Bayeux.Client;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain;
using SoftPhone.Core.Events.Salesforce;
using System;
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
					string selfUri = null;
					string caleeUri = null;

					EventsAggregator.Raise(new SalesforceOutcomingCallEvent(id, selfUri, caleeUri));
				}
			}
		}
	}
}
