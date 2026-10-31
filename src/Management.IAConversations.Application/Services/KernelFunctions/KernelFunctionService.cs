using Management.IAConversations.Domain.Results;
using Microsoft.SemanticKernel;
using Management.IAConversations.Application.Errors;
using Management.IAConversations.Domain.Services.KernelFunction;
using System.Text.Json;

namespace Management.IAConversations.Application.Services.KernelFunctions;

public class KernelFunctionService(Kernel kernel) : IKernelFunctionService
{
	public async Task<Result<string>> ExecuteToolsAsync(string functionName, JsonElement args, CancellationToken cancellationToken)
	{
		KernelFunction? function = null!;

		foreach (var plugin in kernel.Plugins)
		{
			if (plugin.TryGetFunction(functionName, out var foudFunction))
			{
				function = foudFunction;
				break;
			}
		}

		if (function is null)
		{
			return Result<string>.Failure(AiErrors.KernelFunctionNotFoundError);
		}

		var kernelArguments = new KernelArguments();

		foreach (var property in args.EnumerateObject())
		{
			kernelArguments[property.Name] = ConvertJsonValue(property.Value);
		}

		var result = await kernel.InvokeAsync(function, kernelArguments, cancellationToken);

		var output = result.GetValue<object?>();

		return Result<string>.Success(output?.ToString() ?? string.Empty);
	}

	private static object? ConvertJsonValue(JsonElement jsonElement)
	{
		return jsonElement.ValueKind switch
		{
			JsonValueKind.String => jsonElement.GetString(),
			JsonValueKind.True => true,
			JsonValueKind.False => false,
			JsonValueKind.Number when jsonElement.TryGetInt64(out var value) => value,
			JsonValueKind.Number => jsonElement.GetDouble(),
			JsonValueKind.Null => null,
			_ => jsonElement.GetRawText(),

		};
	}
}
