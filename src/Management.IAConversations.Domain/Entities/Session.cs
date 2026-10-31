using MongoDB.Bson.Serialization.Attributes;
using Management.IAConversations.Domain.Enums;

namespace Management.IAConversations.Domain.Entities;

[BsonIgnoreExtraElements]
public sealed class Session
{
	[BsonId]
	public Guid Id { get; private set; } = Guid.NewGuid();

	public string Name { get; private set; } = string.Empty;

	public string PhoneNumber { get; private set; } = string.Empty;

	public string ChatId { get; private set; } = string.Empty;

	public string SessionId { get; private set; } = string.Empty;

	public FileActions FileActions { get; private set; } = FileActions.Pending;

	public Session SetPhoneNumber(string phoneNumber)
	{
		PhoneNumber = phoneNumber;
		return this;
	}

	public Session SetChatId(string chatId)
	{
		ChatId = chatId;
		return this;
	}
	public Session SetName(string name)
	{
		Name = name;
		return this;
	}

	public Session SetSessionId(string sessionId)
	{
		SessionId = sessionId;
		return this;
	}

	public Session SetFileActions(FileActions fileActions)
	{
		FileActions = fileActions;
		return this;
	}

	public Builder ToBuilder() => new()
	{
		Id = Id,
		Name = Name,
		PhoneNumber = PhoneNumber,
		ChatId = ChatId,
		SessionId = SessionId,
		FileActions = FileActions
	};

	public sealed class Builder
	{
		internal Guid Id { get; set; } = Guid.NewGuid();

		internal string Name { get; set; } = string.Empty;

		internal string PhoneNumber { get; set; } = string.Empty;

		internal string ChatId { get; set; } = string.Empty;

		internal string SessionId { get; set; } = string.Empty;

		internal FileActions FileActions { get; set; } = FileActions.Pending;

		public static Builder Create() => new();

		public Builder SetId(Guid id)
		{
			Id = id;
			return this;
		}

		public Builder SetName(string name)
		{
			Name = name;
			return this;
		}

		public Builder SetPhoneNumber(string phoneNumber)
		{
			PhoneNumber = phoneNumber;
			return this;
		}

		public Builder SetChatId(string chatId)
		{
			ChatId = chatId;
			return this;
		}

		public Builder SetSessionId(string sessionId)
		{
			SessionId = sessionId;
			return this;
		}

		public Builder SetFileActions(FileActions fileActions)
		{
			FileActions = fileActions;
			return this;
		}

		public Session Build()
		{
			return new Session
			{
				Id = Id,
				Name = Name,
				PhoneNumber = PhoneNumber,
				ChatId = ChatId,
				SessionId = SessionId,
				FileActions = FileActions
			};
		}
	}
}