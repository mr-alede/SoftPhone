using System;
using Cometd.Client;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;
using System.Collections.Generic;
using System.Net;
using SoftPhone.Salesforce.SalesforceService;
using Cometd.Client.Transport;
using SoftPhone.Core.Core;
using SoftPhone.Core.Events.Salesforce;

namespace SoftPhone.Salesforce.Client
{
	internal class ConnectingSfClient : SfClientStateBase
	{
		private BayeuxClient _client;

		public ConnectingSfClient(SalesforceCredentials credentials)
		{
			System.Threading.Tasks.Task.Run(() =>
			{
				//credentials.Password += "fND1mf1NKH9IKuMfcNEIfICiu"; // salesforce security token
				try
				{
					_client = CreateClient(credentials);

					_client.handshake();
					_client.waitFor(1000, new[] { BayeuxClient.State.CONNECTED });

					_client.getChannel(CHANNEL).subscribe(new SalesforceListener());

					SfClient.State = new ConnectedSfClient(_client);

					EventsAggregator.Raise(new SalesforceClientConnectedEvent());
				}
				catch (Exception ex)
				{
					SfClient.State = new DisconnectedSfClient();

					HandleException(ex);
				}
			});

		}

		public override void Connect(SalesforceCredentials credentials)
		{
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
		}

		private BayeuxClient CreateClient(SalesforceCredentials credentials)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

			var soapClient = new SoapClient();
			var result = soapClient.login(null, credentials.Login, credentials.Password + credentials.SecurityToken);
			if (result.passwordExpired)
				throw new ArgumentOutOfRangeException("Password has expired");

			var options = new Dictionary<String, Object>
			{
				{ ClientTransport.TIMEOUT_OPTION, READ_TIMEOUT }
			};
			HttpClientTransport transport = new LongPollingTransport(options);

			// add the needed auth headers
			//var headers = new NameValueCollection();
			//headers.Add("Authorization", "OAuth " + result.sessionId);
			//transport.AddHeaders(headers);
			transport.setOption(HttpRequestHeader.Authorization.ToString(), "OAuth " + result.sessionId);

			// only need the scheme and host, strip out the rest
			var serverUri = new Uri(result.serverUrl);
			String endpoint = String.Format("{0}://{1}{2}", serverUri.Scheme, serverUri.Host, STREAMING_ENDPOINT_URI);

			return new BayeuxClient(endpoint, new[] { transport });
		}

	}
}