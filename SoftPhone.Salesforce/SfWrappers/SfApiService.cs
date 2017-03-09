using Salesforce.Common.Models.Json;
using Salesforce.Force;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Domain.Salesforce;
using SoftPhone.Core.Events.Salesforce;
using SoftPhone.Core.Repositories.Salesforce;
using SoftPhone.Core.Services;
using SoftPhone.Salesforce.SalesforceService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.SfWrappers
{
	public class SfApiService : ISfApiService
	{
		//public static Dictionary<Guid, List<sObject>> asyncResults;

		//private readonly SoapClient _salesforceService;
		//private readonly SessionHeader _sessionHeader;

		//const int defaultTimeout = 30000;

		//public SfApiService(SalesforceCredentials credentials)
		//{
		//	var connection = new SfConnection(credentials);

		//	this._salesforceService = new SoapClient(); //THIS IS IMPORTANT!!! you can't use the existing instance for some reason...
		//	this._salesforceService.Endpoint.Address = new System.ServiceModel.EndpointAddress(connection.ServerUrl);
		//	_sessionHeader = new SessionHeader();
		//	_sessionHeader.sessionId = connection.SessionID;
		//}

		//public SaveResult[] Update(sObject[] items)
		//{
		//	return _salesforceService.update(items);
		//}

		//public UpsertResult[] Upsert(string externalID, sObject[] items)
		//{
		//	return _salesforceService.upsert(externalID, items);
		//}

		//public SaveResult[] Insert(sObject[] items)
		//{
		//	LimitInfo[] limitInfoHeader;
		//	SaveResult[] result;

		//	_salesforceService
		//		.create(_sessionHeader, null, null, null, null, null, null, null, null, null, null, null, 
		//				items, out limitInfoHeader, out result);

		//	return result;
		//}


		private readonly ISalesforceCredentialsRepository _repo;
		public SfApiService(ISalesforceCredentialsRepository repo)
		{
			_repo = repo;
		}

		public async Task<SuccessResponse> Insert(Conversation conversation)
		{
			var connection = new SfConnection();
			var credentials = _repo.ReadCredentials();
			await connection.Login(credentials);

			var data = new ExpandoObject();

			using (var client = new ForceClient(connection.InstanceUrl, connection.AccessToken, connection.ApiVersion))
			{
				try
				{
					return await client.CreateAsync("Call", data);
				}
				catch (Exception ex)
				{
					HandleException(ex);
				}
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


		//public SaveResult[] Insert(Conversation conversation)
		//{
		//	var sfConversation = new Conversation__c();
		//	sfConversation.Callee__c = conversation.Self.Name;
		//	sfConversation.Caller__c = conversation.Caller.Uri;
		//	sfConversation.Status__c = "Inbound";
		//	sfConversation.Uri__c = conversation.Caller.Uri;

		//	return this.Insert(new sObject[] { sfConversation });
		//}
	}





	//public class SfApiService : IDisposable
	//{
	//public static Dictionary<Guid, List<sObject>> asyncResults;

	//private SoapClient salesforceService;
	//const int defaultTimeout = 30000;

	//public SfApiService()
	//{
	//	salesforceService = new SoapClient();
	//	salesforceService.Timeout = defaultTimeout;
	//	asyncResults = new Dictionary<Guid, List<sObject>>();
	//}

	//public SfApiService(int timeout) : this()
	//{
	//	salesforceService.Timeout = timeout;
	//}

	//public List<T> Query<T>(string soql) where T : sObject, new()
	//{
	//	List<T> returnList = new List<T>();

	//	SetupService();

	//	QueryResult results = salesforceService.query(soql);

	//	for (int i = 0; i < results.size; i++)
	//	{
	//		T item = results.records[i] as T;

	//		if (item != null)
	//			returnList.Add(item);
	//	}

	//	return returnList;
	//}

	//public T QuerySingle<T>(string soql) where T : sObject, new()
	//{
	//	T returnValue = new T();

	//	SetupService();

	//	QueryResult results = salesforceService.query(soql);

	//	if (results.size == 1)
	//		returnValue = results.records[0] as T;

	//	return returnValue;
	//}

	//public Guid QueryAsync(string soql)
	//{
	//	SetupService();
	//	salesforceService.queryCompleted += salesforceService_queryCompleted;

	//	Guid id = Guid.NewGuid();

	//	salesforceService.queryAsync(soql, id);

	//	return id;
	//}

	//void salesforceService_queryCompleted(object sender, queryCompletedEventArgs e)
	//{
	//	Guid id = (Guid)e.UserState;
	//	List<sObject> results = e.Result.records.ToList();

	//	if (asyncResults.ContainsKey(id))
	//		asyncResults[id].AddRange(results);
	//	else
	//		asyncResults.Add((Guid)e.UserState, results);
	//}

	//public SaveResult[] Update(sObject[] items)
	//{
	//	SetupService();

	//	return salesforceService.update(items);
	//}

	//public UpsertResult[] Upsert(string externalID, sObject[] items)
	//{
	//	SetupService();

	//	return salesforceService.upsert(externalID, items);
	//}

	//public SaveResult[] Insert(sObject[] items)
	//{
	//	SetupService();

	//	return salesforceService.create(items);
	//}

	//public DeleteResult[] Delete(string[] ids)
	//{
	//	SetupService();

	//	return salesforceService.delete(ids);
	//}

	//public UndeleteResult[] Undelete(string[] ids)
	//{
	//	SetupService();

	//	return salesforceService.undelete(ids);
	//}

	//private void SetupService()
	//{
	//	var connection = new SfConnection("SalesforceLogin");
	//	salesforceService.SessionHeaderValue =
	//		new SessionHeader() { sessionId = connection.SessionID };

	//	salesforceService.Url = connection.ServerUrl;
	//}

	//public void Dispose()
	//{
	//	salesforceService.Dispose();
	//}
	//}
}
