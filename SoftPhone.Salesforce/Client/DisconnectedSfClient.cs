using System;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;
using System.Collections.Generic;
using System.Net;
using Cometd.Client;
using Cometd.Client.Transport;
using SoftPhone.Salesforce.SalesforceService;

namespace SoftPhone.Salesforce.Client
{
	internal class DisconnectedSfClient : SfClientStateBase
	{
		public override void Connect(SalesforceCredentials credentials)
		{
			try
			{
				BayeuxClient client = CreateClient(credentials);

				client.handshake();
				client.waitFor(1000, new[] { BayeuxClient.State.CONNECTED });

				client.getChannel(CHANNEL).subscribe(new SalesforceListener());

				SfClient.State = new ConnectedSfClient(client);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		public override void Disconnect()
		{
		}

		public override void Push(Conversation conversation)
		{
			throw new InvalidOperationException("Cannot push");
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