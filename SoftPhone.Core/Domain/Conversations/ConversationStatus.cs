using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone.Core.Domain.Conversations
{
	public enum ConversationStatus
	{
		Inbound = 0,
		OutboundSkype = 1,
		OutboundSf = 2,
		Unanswered = 3,
		Finished = 4
	}
}
