using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Salesforce;

namespace SoftPhone.Core.Repositories.Salesforce
{
	public interface ISalesforceCredentialsRepository:IRepository
	{
		SalesforceCredentials ReadCredentials();
		void SaveCredentials(SalesforceCredentials credentials);
	} 
}
