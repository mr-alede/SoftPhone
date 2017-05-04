using SoftPhone.Core.Domain.Conversations;

namespace SoftPhone.Salesforce.SfModel
{
	public class SfCall
	{
		public const string SObjectTypeName = "Call_CxO__c";

		public string Id { get; set; }

		public string Contact_CxO__c { get; set;}
		public string Email_CxO__c { get; set; }
		public string Lead_CxO__c { get; set; }

		public string Number_CxO__c { get; set; }
		public string Status_CxO__c { get; set; }
		public string User_CxO__c { get; set; }

		public SfCall()
		{}

		public SfCall(AppConversation conversation)
		{
			this.Email_CxO__c = AppConversation.Normalize(conversation.Self.Uri);
			this.Number_CxO__c = AppConversation.Normalize(conversation.Other.Uri);
			this.Status_CxO__c = conversation.Status.ToLookupString();
		}
	}
}
