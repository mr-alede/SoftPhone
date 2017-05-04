using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Core.Domain.Conversations
{
	public class AppConversation
	{
		public string Id { get; private set; }
		public string SalesforceId { get; set; }

		public Contact Other { get; set; }

		public Contact Self { get; set; }

		public ConversationStatus Status { get; private set; }

		public AppConversation(string id, ConversationStatus status)
		{
			Id = id;
			Status = status;
		}

		public bool IsExternalCall
		{
			get
			{
				var caller = this.Other;
				if (caller != null && caller.Uri.Contains("tel:"))
					return true;

				return false;
			}
		}

		public static string Normalize(string source)
		{
			if (string.IsNullOrEmpty(source))
				return source;

			var result = source.Replace("sip:", "").Replace("tel:00", "+").Replace("tel:", "");

			int postfixIndex = result.IndexOf(";phone-context");
			if (postfixIndex > -1)
				result = result.Substring(0, postfixIndex);

			return result;
		}

	}
}
