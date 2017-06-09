using Salesforce.Force;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain;
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
		private readonly IAppLogger _logger;

		public SfApiService(ISalesforceCredentialsRepository repo, IAppLogger logger)
		{
			_repo = repo;
			_logger = logger;
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

					_logger.Debug(string.Format("Salesforce call added: self:{0} - other:{1} id: {2}", 
						conversation.Self.Uri,
						conversation.Other.Uri,
						result.Id));

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

				using (var client = new ForceClient(connection.InstanceUrl, connection.AccessToken, connection.ApiVersion))
				{
					var call = await client.QueryByIdAsync<SfCall>(SfCall.SObjectTypeName, conversation.SalesforceId);

					call.Status_CxO__c = conversation.Status.ToLookupString();
					call.Id = null;

					var result = await client.UpdateAsync(SfCall.SObjectTypeName, conversation.SalesforceId, call);

					_logger.Debug(string.Format("Salesforce call updated: self:{0} - other:{1} id: {2}",
						conversation.Self.Uri,
						conversation.Other.Uri,
						conversation.SalesforceId));

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

			_logger.Error(string.Format("Salesforce API exception: {0}", exception.Message));

			EventsAggregator.Raise(new SalesforceClientErrorEvent(exception.Message));
		}
	}
}
