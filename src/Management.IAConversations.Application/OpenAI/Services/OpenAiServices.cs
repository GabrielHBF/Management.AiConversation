using Management.IAConversations.Domain.Results;
using Management.IAConversations.Application.OpenAI.Models.Agent.Events;
using Management.IAConversations.Application.Constants;
using Management.IAConversations.Application.Errors;
using Management.IAConversations.Application.Gateways;
using Management.IAConversations.Domain.Services;
using Management.IAConversations.Domain.Services.KernelFunction;
using System.Text.Json;

namespace Management.IAConversations.Application.OpenAI.Services;

public class OpenAiServices(IChatGptGateway chatGptGateway, IKernelFunctionService kernelFunctionService) : IOpenAiServices
{
	public async Task<Result<string?>> ReadResponseAsync(AgentEventStream stream, string sessionId, CancellationToken cancellationToken = default(CancellationToken))
	{
		string? response = null;

		while (await stream.Reader.ReadLineAsync(cancellationToken) is { } line)
		{
			if (!line.StartsWith(IaConversationsConstants.DataPrefix, StringComparison.Ordinal))
				continue;

			var json = line[IaConversationsConstants.DataPrefix.Length..].Trim();

			if (string.IsNullOrWhiteSpace(json))
				continue;

			using var document = JsonDocument.Parse(json);
			var root = document.RootElement;

			if (!root.TryGetProperty("type", out var typeProperty))
				continue;

			var type = typeProperty.GetString();

			if (type == IaConversationsConstants.RequiredAction)
			{
				var result =
					await RequiredActionsAsync(
						root,
						sessionId,
						cancellationToken);

				if (result.IsFailure)
				{
					return Result<string?>.Failure(
						result.Error);
				}

				continue;
			}

			if (type == IaConversationsConstants.OutputTextDone)
			{
				response = root.TryGetProperty("text", out var textProperty)
					? textProperty.GetString()
					: null;

				return Result<string?>.Success(response);
			}

			if (type is not null && ErrorEventTypes.Contains(type))
			{
				return Result<string?>.Failure(AiErrors.AgentExecutionError);
			}
		}

		return Result<string?>.Success(response);
	}

	private async Task<Result> RequiredActionsAsync(JsonElement root, string sessionId, CancellationToken cancellationToken)
	{
		if (!root.TryGetProperty("session", out var session))
		{
			return Result.Failure(AiErrors.AgentExecutionError);
		}

		if (!session.TryGetProperty("required_actions", out var requiredActions))
		{
			return Result.Failure(AiErrors.AgentExecutionError);
		}

		foreach (var action in requiredActions.EnumerateArray())
		{
			var type = action.GetProperty("type").GetString();

			if (type != "function_call")
				continue;

			var functionName = action.GetProperty("name").GetString();
			var turnId = action.GetProperty("turn_id").GetString();
			var callId = action.GetProperty("call_id").GetString();
			var arguments = action.GetProperty("arguments");

			if (string.IsNullOrEmpty(functionName) ||
				string.IsNullOrEmpty(turnId) ||
				string.IsNullOrEmpty(callId))
			{
				return Result.Failure(AiErrors.AgentExecutionError);
			}

			var functionResult = await kernelFunctionService.ExecuteToolsAsync(
				functionName,
				arguments,
				cancellationToken);

			if (functionResult.IsFailure)
			{
				return Result.Failure(functionResult.Error);
			}

			var sendResult = await chatGptGateway.SendToolResultAsync(
				new(
					sessionId,
					turnId,
					callId,
					functionResult.Value),
				cancellationToken);

			if (sendResult.IsFailure)
			{
				return Result.Failure(sendResult.Error);
			}
		}

		return Result.Success();

	}
	private static readonly HashSet<string> ErrorEventTypes =
		[
			IaConversationsConstants.TurnFailed,
			IaConversationsConstants.TurnCancelled,
			IaConversationsConstants.SessionFailed,
			IaConversationsConstants.EnvironmentFailed,
			IaConversationsConstants.Error
		];
}