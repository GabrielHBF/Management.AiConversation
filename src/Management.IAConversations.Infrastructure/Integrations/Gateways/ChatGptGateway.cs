using Management.IAConversations.Domain.Results;
using Management.IAConversations.Application.OpenAI.Models.Agent.Events;
using Management.IAConversations.Application.OpenAI.Models.Agent.Message;
using Microsoft.Extensions.Logging;
using Management.IAConversations.Application.Constants;
using Management.IAConversations.Application.Errors;
using Management.IAConversations.Application.Gateways;
using Management.IAConversations.Application.OpenAI.Models.Session;
using System.Net.Http.Json;

namespace Management.IAConversations.Infrastructure.Integrations.Gateways;

public class ChatGptGateway(IHttpClientFactory httpClientfactory, ILogger<ChatGptGateway> logger) : IChatGptGateway
{
	private readonly HttpClient _httpClient = httpClientfactory.CreateClient(IaConversationsConstants.ChatGptClient);
	public async Task<Result> SendMessageToAgentSessionAsync(string message, string? sessionId, CancellationToken cancellationToken)
	{
		var messagePayload = new MessagePayload
		{
			Events =
			[
				new AgentSessionEvent
				{
					Type = "agent.session.input.message",
					Input =
					[
						new AgentSessionInput
						{
							Role = "user",
							Content =
							[
								new AgentSessionContent
								{
									Type = "input_text",
									Text = message
								}
							]
						}
					]
				}
			]
		};

		var messageResponse = await _httpClient.PostAsJsonAsync($"agents/sessions/{sessionId}/events",
			messagePayload,
			cancellationToken);

		if (!messageResponse.IsSuccessStatusCode)
		{
			var errorData = await messageResponse.Content.ReadAsStringAsync(cancellationToken);

			logger.LogError("Error when sending message. StatusCode {StatusCode} - Reason {ReasonPhrase} - Response {Response}",
				(int) messageResponse.StatusCode,
				messageResponse.ReasonPhrase,
				errorData);

			AiErrors.SetTechnicalMessage(errorData);

			return Result.Failure(AiErrors.MessageSendingError);
		}

		return Result.Success();
	}

	public async Task<Result<SessionResponse>> GetNewSessionIdAsync(string agentId, CancellationToken cancellationToken)
	{
		var sessionPayload = new SessionPayload
		{
			AgentId = agentId,
			Environment = new EnvironmentPayload
			{
				Type = "openai_hosted"
			}
		};

		var sessionReponse = await _httpClient.PostAsJsonAsync("agents/sessions", sessionPayload, cancellationToken);

		if (!sessionReponse.IsSuccessStatusCode)
		{
			var errorData = await sessionReponse.Content.ReadAsStringAsync(cancellationToken);

			logger.LogError("Error when creating session. StatusCode {StatusCode} - Reason {ReasonPhrase} - Response {Response}",
				(int) sessionReponse.StatusCode,
				sessionReponse.ReasonPhrase,
				errorData);

			AiErrors.SetTechnicalMessage(errorData);

			return Result<SessionResponse>.Failure(AiErrors.SessionCreationError);
		}

		var sessionData = await sessionReponse.Content.ReadFromJsonAsync<SessionResponse>(cancellationToken);

		return Result<SessionResponse>.Success(sessionData!);
	}

	public async Task<Result<AgentEventStream>> OpenEventStreamAsync(string sessionId, CancellationToken cancellationToken)
	{
		var request = new HttpRequestMessage(HttpMethod.Get,
			$"agents/sessions/{sessionId}/events?stream=true");

		request.Headers.Accept.Add(
			new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

		var response = await _httpClient.SendAsync(request,
			HttpCompletionOption.ResponseHeadersRead, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			var errorData = await response.Content.ReadAsStringAsync(cancellationToken);

			logger.LogError("Error when retrieving agent event stream. StatusCode {StatusCode} - Reason {ReasonPhrase} - Response {Response}",
				(int) response.StatusCode,
				response.ReasonPhrase,
				errorData);

			AiErrors.SetTechnicalMessage(errorData);

			return Result<AgentEventStream>.Failure(AiErrors.EventStreamError);
		}

		var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

		var reader = new StreamReader(stream);

		return Result<AgentEventStream>.Success(new(response, reader));
	}

	public async Task<Result> SendToolResultAsync(AgentTool agentTool, CancellationToken cancellationToken)
	{
		var payload = new MessagePayload
		{
			Events =
		[
			new AgentSessionEvent
			{
				Type = "agent.session.input.tool_result",
				TurnId = agentTool.TurnId,
				CallId = agentTool.CallId,
				Success = true,
				Output = agentTool.Output
			}
		]
		};

		var response = await _httpClient.PostAsJsonAsync($"agents/sessions/{agentTool.SessionId}/events",
			payload,
			cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			var errorData = await response.Content.ReadAsStringAsync(
				cancellationToken);

			logger.LogError("Error when sending tool result to agent session {SessionId}. StatusCode {StatusCode} - Reason {ReasonPhrase} - Response {Response}",
				agentTool.SessionId,
				(int) response.StatusCode,
				response.ReasonPhrase,
				errorData);

			AiErrors.SetTechnicalMessage(errorData);

			return Result.Failure(AiErrors.ToolResultSendingError);
		}

		return Result.Success();
	}
}