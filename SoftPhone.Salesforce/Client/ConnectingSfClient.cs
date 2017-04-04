using System;
using Cometd.Client;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;
using System.Collections.Generic;
using System.Net;
using Cometd.Client.Transport;
using SoftPhone.Core.Core;
using SoftPhone.Core.Events.Salesforce;
using SoftPhone.Salesforce.SfWrappers;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.Client
{
	internal class ConnectingSfClient : SfClientStateBase
	{

		public ConnectingSfClient(SalesforceCredentials credentials)
		{
			CreateClient(credentials).ContinueWith(result => 
			{
				if (result.IsFaulted)
				{
					SfClient.State = new DisconnectedSfClient();
					HandleException(result.Exception);
					return;
				}

				var client = result.Result;

				client.handshake();
				var state = client.waitFor(1000, new[] { BayeuxClient.State.CONNECTED });

				client.getChannel(CHANNEL).subscribe(new SalesforceListener());

				SfClient.State = new ConnectedSfClient(client, credentials);

				EventsAggregator.Raise(new SalesforceClientConnectedEvent());

			});
		}

		public override void Connect(SalesforceCredentials credentials)
		{
		}

		public override void Disconnect()
		{
		}

		private async Task<BayeuxClient> CreateClient(SalesforceCredentials credentials)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


			var connection = new SfConnection();
			await connection.Login(credentials);

			var options = new Dictionary<String, Object>
			{
				{ ClientTransport.TIMEOUT_OPTION, READ_TIMEOUT }
			};
			HttpClientTransport transport = new LongPollingTransport(options);

			// add the needed auth headers
			//var headers = new NameValueCollection();
			//headers.Add("Authorization", "OAuth " + result.sessionId);
			//transport.AddHeaders(headers);
			transport.setOption(HttpRequestHeader.Authorization.ToString(), "OAuth " + connection.AccessToken);

			// only need the scheme and host, strip out the rest
			var serverUri = new Uri(connection.InstanceUrl);
			String endpoint = String.Format("{0}://{1}{2}", serverUri.Scheme, serverUri.Host, STREAMING_ENDPOINT_URI);

			return new BayeuxClient(endpoint, new[] { transport });
		}

	}
}