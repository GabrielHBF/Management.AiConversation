using Management.IAConversations.Application.Response;

namespace Management.IAConversations.Application.Gateways;

public interface IOpenAiDocumentValidator
{
	Task<string> ValidateAsync(string fileName, string contentType, byte[] fileContent, CancellationToken cancellationToken);
}