using Management.IAConversations.Application.Constants;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using System.Text;

namespace Management.IAConversations.Infrastructure.Integrations.Kernels.Agents;

public class AsaCodeAgent(Kernel kernel)
{
	private readonly ChatCompletionAgent _agent = new()
	{
		Name = IaConversationsConstants.Agents.AsaCode,
		Instructions = Path.Combine(AppContext.BaseDirectory, "Agents", "Prompts", "ASA_CodePrompt.md"),
		Kernel = kernel,
		Arguments = new KernelArguments(
			new PromptExecutionSettings
			{
				ServiceId = IaConversationsConstants.NvidiaAiClient
			})
	};

	public async Task<string> ExecuteAsync(string description, CancellationToken cancellationToken = default)
	{
		var result = new StringBuilder();

		await foreach (var response in _agent.InvokeAsync(description, cancellationToken: cancellationToken))
		{
			result.Append(response.Message.Content);
		}

		return result.ToString();
	}
}