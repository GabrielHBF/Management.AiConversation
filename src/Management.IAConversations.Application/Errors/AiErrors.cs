using Management.IAConversations.Domain.Results;

namespace Management.IAConversations.Application.Errors;

public static class AiErrors
{
	public static string TechnicalMessage { get; private set; } = string.Empty;
	public static Error SessionCreationError => new("SessionCreationError", $"An error occurred while creating the user's automatic session. Message : {TechnicalMessage}");
	public static Error MessageSendingError => new("MessageSendingError", $"An error occurred while sending the message to the user's session. {TechnicalMessage}");
	public static Error EventStreamError => new("EventStreamError", $"An error occurred while retrieving the event stream for the user's session. {TechnicalMessage}");
	public static Error ToolResultSendingError => new("ToolResultSendingError", $"An error occurred while sending the tool result to the agent's session. {TechnicalMessage}");
	public static Error AgentExecutionError => new("AgentExecutionError", $"An error occurred while executing the AI agent. {TechnicalMessage}");
	public static Error AgentNotFoundError => new("AgentNotFoundError", $"The specified agent could not be found. {TechnicalMessage}");
	public static Error AgentAlreadyExistsError => new("AgentAlreadyExistsError", $"The specified agent already exists and cannot be added again. {TechnicalMessage}");
	public static Error KernelFunctionNotFoundError => new("KernelFunctionNotFoundError", $"The requested kernel function was not found. Message : {TechnicalMessage}");

	public static void SetTechnicalMessage(string technicalMessage)
	{
		TechnicalMessage = technicalMessage;
	}
}