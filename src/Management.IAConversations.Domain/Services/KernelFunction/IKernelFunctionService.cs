using Management.IAConversations.Domain.Results;
using System.Text.Json;

namespace Management.IAConversations.Domain.Services.KernelFunction;

public interface IKernelFunctionService
{
	Task<Result<string>> ExecuteToolsAsync(string functionName, JsonElement args, CancellationToken cancellationToken = default);
}