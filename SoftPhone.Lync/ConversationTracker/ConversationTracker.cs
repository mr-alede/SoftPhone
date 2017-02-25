using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using SoftPhone.Core.Core;
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
			string id = conversation.Properties[ConversationProperty.Id].ToString();
			var appConversation = new Core.Domain.Conversations.Conversation(id);

			appConversation.Contacts = conversation.Participants
				.Where(x => !x.IsSelf)
				.Select(x => CreateContact(x))
			.ToList();

			appConversation.Self = conversation.Participants
				.Where(x => x.IsSelf)
				.Select(x => CreateContact(x))
				.First();

			EventsAggregator.Raise(new Core.Domain.Conversations.ConversationAddedEvent(appConversation));
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

		private static Core.Domain.Conversations.Contact CreateContact(Participant participant)
		{
			var endpoints = (_client.ContactManager.GetContactByUri(participant.Contact.Uri).GetContactInformation(ContactInformationType.ContactEndpoints) as ICollection).OfType<ContactEndpoint>().ToList();

			return new Core.Domain.Conversations.Contact
			{
				Uri = participant.Contact.Uri,
				Name = _client.ContactManager.GetContactByUri(participant.Contact.Uri).GetContactInformation(ContactInformationType.DisplayName).ToString(),
				Endpoints = endpoints.Select(e => new Core.Domain.Conversations.ContactEndpoint
				{
					Name = e.DisplayName,
					Uri = e.Uri,
					Type = (Core.Domain.Conversations.EndpointType)e.Type
				}).ToList()
			};

		}
	}
}
