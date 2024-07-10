using ECraft.Contracts.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECraft.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles ="Crafter")]
	public class ProjectController : ControllerBase
	{
		[HttpPost("project")]
		[Authorize]
		public async Task<IActionResult> AddProject([FromBody] CrafterProfileRequest crafterInfo)
		{

			return Ok("Under Construction");
		}

		[HttpPatch("project")]
		[Authorize]
		public async Task<IActionResult> EditProject([FromBody] CrafterProfileRequest crafterInfo)
		{
			return Ok("Under Construction");
		}
	}
}
