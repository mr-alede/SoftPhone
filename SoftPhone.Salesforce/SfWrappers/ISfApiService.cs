using Salesforce.Common.Models.Json;
using SoftPhone.Core.Domain.Conversations;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.SfWrappers
{
	public interface ISfApiService
	{
		Task<SuccessResponse> Insert(Conversation conversation);
	}

}
