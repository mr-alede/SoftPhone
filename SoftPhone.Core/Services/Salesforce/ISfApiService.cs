using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using System.Threading.Tasks;

namespace SoftPhone.Core.Services.Salesforce
{
	public interface ISfApiService: IService
	{
		Task<AppConversation> Insert(AppConversation conversation);
	}

}
