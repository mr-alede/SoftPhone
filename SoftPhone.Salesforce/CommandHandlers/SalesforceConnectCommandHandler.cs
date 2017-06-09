using Cometd.Client;
using Cometd.Client.Transport;
using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain;
using SoftPhone.Core.Domain.Salesforce;
using SoftPhone.Core.Events.Salesforce;
using SoftPhone.Core.Repositories.Salesforce;
using SoftPhone.Salesforce.Client;
using SoftPhone.Salesforce.SfWrappers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.CommandHandlers
{
	public class SalesforceConnectCommandHandler : ICommandHandler<SalesforceConnectCommand>
	{
		private static BayeuxClient client;

		private readonly ISalesforceCredentialsRepository _repo;
		private readonly IAppLogger _logger;

		private const String CHANNEL = "/topic/Call_All";
		private const String STREAMING_ENDPOINT_URI = "/cometd/36.0";
		// long pull durations
		public const int READ_TIMEOUT = 120 * 1000;
		public const int THREAD_TIMEOUT = 60 * 1000;

		static SalesforceConnectCommandHandler()
		{
			// SF requires TLS 1.1 or 1.2
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
		}

		public SalesforceConnectCommandHandler(ISalesforceCredentialsRepository repo, IAppLogger logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public void Execute(SalesforceConnectCommand command)
		{
			if (client != null)
			{
				client.disconnect();
				client = null;
			}

			var credentials = _repo.ReadCredentials();
			CreateClient(credentials).ContinueWith(result =>
			{
				if (result.IsFaulted)
				{
					HandleException(result.Exception);
					return;
				}

				client = result.Result;

				client.handshake();
				var state = client.waitFor(1000, new[] { BayeuxClient.State.CONNECTED });

				client.getChannel(CHANNEL).subscribe(new SalesforceListener());

				_logger.Debug("Connected to salesforce");

				EventsAggregator.Raise(new SalesforceClientConnectedEvent());
			});

		}

		private async Task<BayeuxClient> CreateClient(SalesforceCredentials credentials)
		{
			var connection = new SfConnection();
			await connection.Login(credentials);

			var options = new Dictionary<String, Object>
				{
					{ ClientTransport.TIMEOUT_OPTION, READ_TIMEOUT },
					{ HttpRequestHeader.Authorization.ToString(), "OAuth " + connection.AccessToken}
				};

			var transport = new LongPollingTransport(options);

			// only need the scheme and host, strip out the rest
			var serverUri = new Uri(connection.InstanceUrl);
			String endpoint = String.Format("{0}://{1}{2}", serverUri.Scheme, serverUri.Host, STREAMING_ENDPOINT_URI);

			return new BayeuxClient(endpoint, new[] { transport });
		}

		protected void HandleException(Exception e)
		{
			Exception exception = e;
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
			}

			_logger.Error("Salesforce push notifications exception: " + exception.Message);

			EventsAggregator.Raise(new SalesforceClientErrorEvent(exception.Message));
		}

	}

}
