using ECraft.Contracts.Request;
using ECraft.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ECraft.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles ="Crafter")]
	public class CrafterController : ControllerBase
	{
		private readonly AppDbContext _db;

		public CrafterController(AppDbContext db)
		{
			_db = db;
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateAccount([FromBody] CrafterProfileRequest crafterInfo)
		{
			return Ok("Under Construction");
		}


		[HttpPatch]
		[Authorize]
		public async Task<IActionResult> EditAccount([FromBody] JsonPatchDocument<CrafterProfileRequest> crafterPatchDoc)
		{
			return Ok("Under Construction");
		}
	}
}
