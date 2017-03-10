﻿using SoftPhone.Core.Domain.Conversations;

namespace SoftPhone.Salesforce.SfModel
{
	public class SfCall
	{
		public const string SObjectTypeName = "Call__c";

		public string Id { get; set; }

		public string Contact__c { get; set;}
		public string Email__c { get; set; }
		public string Lead__c { get; set; }

		public string Number__c { get; set; }
		public string Status__c { get; set; }
		public string User__c { get; set; }


		public SfCall(Conversation conversation, ConversationStatus status)
		{
			this.Email__c = conversation.Self.Uri;
			this.Number__c = conversation.Caller.Uri;
			this.Status__c = status.ToString();
		}
	}
}