using Salesforce.Force;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Events.Salesforce;
using SoftPhone.Core.Repositories.Salesforce;
using SoftPhone.Core.Services.Salesforce;
using SoftPhone.Salesforce.SfModel;
using SoftPhone.Salesforce.SfWrappers;
using System;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.Services
{
	public class SfApiService : ISfApiService
	{
		private readonly ISalesforceCredentialsRepository _repo;
		public SfApiService(ISalesforceCredentialsRepository repo)
		{
			_repo = repo;
		}

		public async Task<AppConversation> Insert(AppConversation conversation)
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
					conversation.SalesforceId = result.Id;

					return conversation;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
		}

		public async Task<AppConversation> Update(AppConversation conversation)
		{
			try
			{
				var connection = new SfConnection();
				var credentials = _repo.ReadCredentials();
				await connection.Login(credentials);

				var call = new SfCall(conversation);
				call.Id = null;

				using (var client = new ForceClient(connection.InstanceUrl, connection.AccessToken, connection.ApiVersion))
				{
					var result = await client.UpdateAsync(SfCall.SObjectTypeName, conversation.SalesforceId, call);

					return conversation;
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
