using Cometd.Bayeux;
using Cometd.Bayeux.Client;
using System;

namespace SoftPhone.Salesforce.Client
{
	class SalesforceListener : IMessageListener
	{
		public void onMessage(IClientSessionChannel channel, IMessage message)
		{
			Console.WriteLine(message);
		}
	}
}
