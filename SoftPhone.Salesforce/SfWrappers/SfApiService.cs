using SoftPhone.Salesforce.SalesforceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.SfWrappers
{
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
