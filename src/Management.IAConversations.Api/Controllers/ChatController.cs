using HelpWaiter.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Management.IAConversations.Application.Input.Commands;
using Management.IAConversations.Application.Request;

namespace Management.IAConversations.Api.Controllers;

[Route("api/v1/chat")]
[ApiController]
public class ChatController(IWaiter commandMediator) : ControllerBase
{
	private readonly IWaiter _commandMediator = commandMediator;

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> SendMessage([FromBody] SendConversationRequest message, CancellationToken cancellationToken)
	{
		var conversationId = Request.Cookies["conversation_id"];

		var result = await _commandMediator.SendCommandAsync(new
			SendConversationMessageCommand(message, conversationId),
			cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		if (result.Value.ConversationId is not null &&
			conversationId is null)
		{
			Response.Cookies.Append("conversation_id",
				result.Value.ConversationId,
				new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.Lax,
					Expires = DateTime.UtcNow.AddDays(30)
				});
		}

		return Ok(result.Value);
	}
}