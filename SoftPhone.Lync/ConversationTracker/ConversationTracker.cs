using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using SoftPhone.Core.DomainEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Lync.ConversationTracker
{
	public class ConversationTracker
	{
		static Dictionary<String, ConversationContainer> ActiveConversations = new Dictionary<String, ConversationContainer>();
		static LyncClient _client;

		public static void Init()
		{
			_client = LyncClient.GetClient();
			_client.ConversationManager.ConversationAdded += ConversationManager_ConversationAdded;
			_client.ConversationManager.ConversationRemoved += ConversationManager_ConversationRemoved;
		}

		static void ConversationManager_ConversationAdded(object sender, ConversationManagerEventArgs e)
		{
			string ConversationID = e.Conversation.Properties[ConversationProperty.Id].ToString();

			if (e.Conversation.Modalities[ModalityTypes.AudioVideo].State != ModalityState.Disconnected)
			{
				StoreConversation(e.Conversation, ConversationID);
			}
			else
			{
				e.Conversation.Modalities[ModalityTypes.AudioVideo].ModalityStateChanged += Program_ModalityStateChanged;
			}
		}

		static void Program_ModalityStateChanged(object sender, ModalityStateChangedEventArgs e)
		{
			//in this case, any state change will be from Disconnected and will therefore indicate some A/V activity
			var modality = sender as Microsoft.Lync.Model.Conversation.AudioVideo.AVModality;

			string ConversationID = modality.Conversation.Properties[ConversationProperty.Id].ToString();

			if (!ActiveConversations.ContainsKey(ConversationID))
			{
				StoreConversation(modality.Conversation, ConversationID);
				modality.ModalityStateChanged -= Program_ModalityStateChanged;
			}
		}

		private static void GenerateConversationAddedEvent(Conversation conversation)
		{
			var appConversation = new Core.Conversations.Conversation();

			appConversation.Contacts = conversation.Participants
				.Where(x => !x.IsSelf)
				.Select(x =>
				{
					var endpoints = (_client.ContactManager.GetContactByUri(x.Contact.Uri).GetContactInformation(ContactInformationType.ContactEndpoints) as ICollection).OfType<ContactEndpoint>().ToList();

					return new Core.Conversations.Contact
					{
						Uri = x.Contact.Uri,
						Name = _client.ContactManager.GetContactByUri(x.Contact.Uri).GetContactInformation(ContactInformationType.DisplayName).ToString(),
						Endpoints = endpoints.Select(e => new Core.Conversations.ContactEndpoint
						{
							Name = e.DisplayName,
							Uri = e.Uri,
							Type = (Core.Conversations.EndpointType)e.Type
						}).ToList()
					};
				})
			.ToList();

			EventsAggregator.Raise(new Core.Conversations.ConversationAddedEvent(appConversation));
		}

		private static void StoreConversation(Conversation conversation, string ConversationID)
		{
			ActiveConversations.Add(ConversationID, new ConversationContainer()
			{
				Conversation = conversation,
				ConversationCreated = DateTime.Now
			});

			GenerateConversationAddedEvent(conversation);
		}

		static void ConversationManager_ConversationRemoved(object sender, Microsoft.Lync.Model.Conversation.ConversationManagerEventArgs e)
		{
			string ConversationID = e.Conversation.Properties[ConversationProperty.Id].ToString();
			if (ActiveConversations.ContainsKey(ConversationID))
			{
				var container = ActiveConversations[ConversationID];
				TimeSpan conversationLength = DateTime.Now.Subtract(container.ConversationCreated);
				Console.WriteLine("Conversation {0} lasted {1} seconds", ConversationID, conversationLength);
				ActiveConversations.Remove(ConversationID);
			}
		}
	}
}
