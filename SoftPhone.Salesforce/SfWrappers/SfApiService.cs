using Salesforce.Common.Models.Json;
using Salesforce.Force;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Events.Salesforce;
using SoftPhone.Core.Repositories.Salesforce;
using SoftPhone.Salesforce.SfModel;
using System;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.SfWrappers
{
	public class SfApiService : ISfApiService
	{
		private readonly ISalesforceCredentialsRepository _repo;
		public SfApiService(ISalesforceCredentialsRepository repo)
		{
			_repo = repo;
		}

		public async Task<SuccessResponse> Insert(AppConversation conversation)
		{
			try
			{
				var connection = new SfConnection();
				var credentials = _repo.ReadCredentials();
				await connection.Login(credentials);

				var call = new SfCall(conversation);
				using (var client = new ForceClient(connection.InstanceUrl, connection.AccessToken, connection.ApiVersion))
				{
					var result = await client.CreateAsync(SfCall.SObjectTypeName, call);

					return result;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
		}

		protected void HandleException(Exception e)
		{
			Exception exception = e;
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
			}

			EventsAggregator.Raise(new SalesforceClientErrorEvent(exception.Message));
		}
	}
}
