using HelpWaiter.Abstractions;
using Management.IAConversations.Application.Input.Commands;
using Management.IAConversations.Application.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Management.IAConversations.Api.Controllers
{
	[Route("api/v1/agent")]
	[ApiController]
	public class AgentController(IWaiter commandMediator) : ControllerBase
	{
		private readonly IWaiter _commandMediator = commandMediator;

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddAgent([FromBody] AddAgentRequest request, CancellationToken cancellationToken)
		{
			var result = await _commandMediator.SendCommandAsync(new AddAgentCommand(request), cancellationToken);

			if (result.IsFailure)
			{
				return BadRequest(result.Error);
			}

			return Ok();
		}
	}
}