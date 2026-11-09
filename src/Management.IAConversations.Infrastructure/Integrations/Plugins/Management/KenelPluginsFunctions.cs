using Management.IAConversations.Infrastructure.Integrations.Kernels.Agents;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Management.IAConversations.Infrastructure.Integrations.Plugins.Management;

public sealed class KernelPluginsFunctions(AsaCodeAgent asaCodeAgent)
{
	[KernelFunction("gerar_codigo")]
	[Description("Delegue ao agente especialista ASA_Code a tarefa de analisar e gerar código.")]
	public Task<string> GenerateCodeAsync(
		[Description("Descrição detalhada da tarefa de desenvolvimento.")]
		string description,
		CancellationToken cancellationToken = default)
	{
		return asaCodeAgent.ExecuteAsync(description, cancellationToken);
	}
}