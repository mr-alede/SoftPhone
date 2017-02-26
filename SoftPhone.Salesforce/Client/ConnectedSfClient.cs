using System;
using Cometd.Client;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;
using System.Collections.Generic;
using SoftPhone.Salesforce.SalesforceService;

namespace SoftPhone.Salesforce.Client
{
	internal class ConnectedSfClient : SfClientStateBase
	{
		private readonly BayeuxClient _client;
		private readonly SoapClient _soapClient;

		public ConnectedSfClient(BayeuxClient client, SoapClient soapClient)
		{
			_client = client;
			_soapClient = soapClient;
		}

		public override void Connect(SalesforceCredentials credentials)
		{
			throw new InvalidOperationException("Cannot connect");
		}

		public override void Disconnect()
		{
			try
			{
				_client.disconnect();
				_client.waitFor(1000, new[] { BayeuxClient.State.DISCONNECTED });

				SfClient.State = new DisconnectedSfClient();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		public override void Push(Conversation conversation)
		{
			try
			{

				// Publishing to channels
				var data = new Dictionary<String, Object>();
				data.Add("conversation", conversation);
				_client.getChannel(CHANNEL).publish(data);

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
	}
}