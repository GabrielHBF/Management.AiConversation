using Management.IAConversations.Application.Context;
using Management.IAConversations.Domain.Interfaces;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Management.IAConversations.Infrastructure.Integrations.Plugins.Governaments;

public class KenelPluginsFunctions(ISessionRespository sessionRespository,
	ConversationContext conversationContext)
{
	[KernelFunction("get_session")]
	[Description("Obtém informações de uma sessão.")]
	public async Task<string> GetSessionAsync(string sessionId, CancellationToken cancellationToken)
	{
		return "Informações da sessão";
	}
}