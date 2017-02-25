using System;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Salesforce.Client
{
	public abstract class SfClientStateBase
	{
		public const String CHANNEL = "/topic/InvoiceStatementUpdates";
		public const String STREAMING_ENDPOINT_URI = "/cometd/29.0";

		// long pull durations
		public const int READ_TIMEOUT = 120 * 1000;
		public const int THREAD_TIMEOUT = 60 * 1000;

		public abstract void Connect(SalesforceCredentials credentials);

		public abstract void Disconnect();

		public abstract void Push(Conversation conversation);

		protected void HandleException(Exception e)
		{
			Exception innerException = e.InnerException;
			while (innerException != null)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);

				innerException = innerException.InnerException;
			}
		}
	}
}
