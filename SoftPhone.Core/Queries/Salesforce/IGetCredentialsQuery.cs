using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Core.Queries.Salesforce
{
	public interface IGetCredentialsQuery: IQuery
	{
		SalesforceCredentials Execute();
	}
}
