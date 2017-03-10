using Salesforce.Common.Models.Json;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.SfWrappers
{
	public interface ISfApiService: IService
	{
		Task<SuccessResponse> Insert(Conversation conversation, ConversationStatus status);
	}

}
