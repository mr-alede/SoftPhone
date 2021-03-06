﻿using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Events.Lync;
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
			var modalityState = e.Conversation.Modalities[ModalityTypes.AudioVideo].State;

			if (modalityState != ModalityState.Disconnected)
			{
				StoreConversation(e.Conversation, ConversationID);
				e.Conversation.Modalities[ModalityTypes.AudioVideo].ModalityStateChanged += Program_ModalityStateChanged;
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
				//modality.ModalityStateChanged -= Program_ModalityStateChanged;
			}

			GenerateConversationAddedEvent(modality.Conversation, e);

			GenerateConversationTerminatedEvent(modality.Conversation, e);
		}

		private static AppConversation CreateAppConversation(Conversation conversation, ConversationStatus status)
		{
			string id = conversation.Properties[ConversationProperty.Id].ToString();
			var appConversation = new AppConversation(id, status);

			var contacts = conversation.Participants
				.Where(x => !x.IsSelf)
				.Select(x => CreateContact(x))
				.ToList();

			appConversation.Self = conversation.Participants
				.Where(x => x.IsSelf)
				.Select(x => CreateContact(x))
				.FirstOrDefault();

			if(appConversation.Self == null)
			{
				appConversation.Self = CreateContact(_client.Self.Contact);
			}

			appConversation.Other = contacts.Where(x => x.Uri != appConversation.Self.Uri).FirstOrDefault();

			return appConversation;
		}

		private static void GenerateConversationAddedEvent(Conversation conversation, ModalityStateChangedEventArgs e)
		{
			if (e.OldState == ModalityState.Disconnected && e.NewState == ModalityState.Connecting)
			{
				var appConversation = CreateAppConversation(conversation, ConversationStatus.OutboundSkype);
				EventsAggregator.Raise(new Core.Domain.Conversations.ConversationEvent(appConversation));
			}

			if (e.OldState == ModalityState.Disconnected && e.NewState == ModalityState.Notified)
			{
				var appConversation = CreateAppConversation(conversation, ConversationStatus.Inbound);
				EventsAggregator.Raise(new Core.Domain.Conversations.ConversationEvent(appConversation));
			}

			//if (e.OldState == ModalityState.Notified && e.NewState == ModalityState.Joining)
			//{
			//	var appConversation = CreateAppConversation(conversation, ConversationStatus.Inbound);
			//	EventsAggregator.Raise(new Core.Domain.Conversations.ConversationEvent(appConversation));
			//}
		}

		private static void GenerateConversationTerminatedEvent(Conversation conversation, ModalityStateChangedEventArgs e)
		{
			if (e.OldState == ModalityState.Connected && e.NewState == ModalityState.Disconnected)
			{
				var appConversation = CreateAppConversation(conversation, ConversationStatus.Finished);
				EventsAggregator.Raise(new Core.Domain.Conversations.ConversationEvent(appConversation));
			}

			if (e.OldState != ModalityState.Connected && e.NewState == ModalityState.Disconnected)
			{
				var appConversation = CreateAppConversation(conversation, ConversationStatus.Unanswered);
				EventsAggregator.Raise(new Core.Domain.Conversations.ConversationEvent(appConversation));
			}
		}

		private static void StoreConversation(Conversation conversation, string ConversationID)
		{
			ActiveConversations.Add(ConversationID, new ConversationContainer()
			{
				Conversation = conversation,
				ConversationCreated = DateTime.Now
			});
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

			//GenerateConversationTerminatedEvent(e.Conversation);
		}

		private static Core.Domain.Conversations.Contact CreateContact(Participant participant)
		{
			var endpoints = (_client.ContactManager
								.GetContactByUri(participant.Contact.Uri)
								.GetContactInformation(ContactInformationType.ContactEndpoints) as ICollection
							)
							.OfType<Microsoft.Lync.Model.ContactEndpoint>()
							.ToList();

			return new Core.Domain.Conversations.Contact
			{
				Uri = participant.Contact.Uri,
				Name = _client.ContactManager.GetContactByUri(participant.Contact.Uri).GetContactInformation(ContactInformationType.DisplayName).ToString(),
				//Endpoints = endpoints.Select(e => new Core.Domain.Conversations.ContactEndpoint
				//{
				//	Name = e.DisplayName,
				//	Uri = e.Uri,
				//	Type = (Core.Domain.Conversations.EndpointType)e.Type
				//}).ToList()
			};

		}

		private static Core.Domain.Conversations.Contact CreateContact(Microsoft.Lync.Model.Contact contact)
		{
			return new Core.Domain.Conversations.Contact
			{
				Uri = contact.Uri,
				Name = _client.ContactManager.GetContactByUri(contact.Uri).GetContactInformation(ContactInformationType.DisplayName).ToString(),
			};
		}

		private static void HandleException(Exception e)
		{
			Exception exception = e;
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
			}

			EventsAggregator.Raise(new LyncClientErrorEvent(exception.Message));
		}

	}
}
