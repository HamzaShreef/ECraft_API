using ECraft.Constants;
using ECraft.Contracts.LocationDto;
using ECraft.Data;
using ECraft.Extensions;
using ECraft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECraft.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class LocationController : ControllerBase
	{
		private readonly AppDbContext _db;

		public LocationController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet("country/{countryId}")]
		public async Task<IActionResult> GetCountry(int countryId)
		{
			LocationCountry countryWithCities = await _db.LCountries.Where(c => c.Id == countryId)
				.Include(c => c.CountryCities)
				.Include(c => c.CountryStates)
				.FirstOrDefaultAsync();

			if (countryWithCities is null)
				return NotFound("Invalid CountryId");

			CountryDto countryDto = new CountryWithCitiesDto()
			{
				CountryStates = countryWithCities.CountryStates.ToList().Select(s => new StateDto().GetDto(s)),
				CountryCities = countryWithCities.CountryCities.ToList().Select(c => new CityDto().GetDto(c)),
				citiesCount=countryWithCities.CountryCities.Count,
				statesCount=countryWithCities.CountryStates.Count
			}.GetDto(countryWithCities);


			return Ok(countryDto);
		}

		[HttpGet("state/{stateId}")]
		public async Task<IActionResult> GetState(int stateId)
		{
			LocationState stateWithCities = await _db.LStates.Where(s => s.Id == stateId)
				.Include(s => s.StateCities)
				.FirstOrDefaultAsync();

			if (stateWithCities is null)
				return NotFound("Invalid StateId");

			StateDto stateDto = new StateWithCitiesDto()
			{
				StateCities = stateWithCities.StateCities.ToList().Select(c => new CityDto().GetDto(c)),
				CitiesCount=stateWithCities.StateCities.Count
			}.GetDto(stateWithCities);


			return Ok(stateDto);
		}

		[HttpGet("country")]
		public async Task<IActionResult> GetAllCountries()
		{
			var countries = await _db.LCountries.Select<LocationCountry, CountryDto>(c => new CountryDto
			{
				CountryName = c.CountryName,
				CountryCode = c.CountryCode,
				CountryId = c.Id
			}).ToListAsync();

			if(countries == null || countries.Count == 0)
				return NotFound();

			return Ok(countries);

		}

		[HttpPost("country")]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> AddCountry([FromBody] CountryDto countryDto)
		{
			if (ModelState.IsValid)
			{
				LocationCountry newCountryRecord = countryDto.GetDomainEntity(out bool successfulMapping, out IdentityError? validationError);

				_db.LCountries.Add(newCountryRecord);

				try
				{
					await _db.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Service is down");
				}

				countryDto.CountryId = newCountryRecord.Id;
				return Ok(countryDto);
			}
			else
				return BadRequest(ModelState.GetErrorList());

		}

		[HttpPatch("country")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditCountry()
		{
			return Ok("Under Construction");
		}

		[HttpPost("state")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddState([FromBody] StateDto stateDto)
		{
			if (ModelState.IsValid)
			{
				int countryId = stateDto.CountryId;

				bool validCountryId = await _db.LCountries.AnyAsync(c => c.Id == countryId);

				if (!validCountryId)
					return NotFound("CountryId does not match a stored record");

				

				LocationState newStateRecord = stateDto.GetDomainEntity(out bool successfulMapping, out IdentityError? validationError);

				_db.LStates.Add(newStateRecord);

				try
				{
					await _db.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					IdentityError error = new IdentityError()
					{
						Code = GeneralErrorCodes.ServiceDown,
						Description = ex.Message

					};
					
					var errors = new List<IdentityError>() { error };
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}

				stateDto.StateId = newStateRecord.Id;
				return Ok(stateDto);
			}
			else
				return BadRequest(ModelState.GetErrorList());
		}

		[HttpPatch("state")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditState()
		{
			return Ok("Under Construction");
		}

		[HttpDelete("state")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteState()
		{
			return Ok("Under Construction");
		}

		[HttpPost("city")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddCity([FromBody] CityDto cityDto)
		{
			if (ModelState.IsValid)
			{
				int countryId = cityDto.CountryId;
				int? stateId = cityDto.StateId;


				bool validCountryId = await _db.LCountries.AnyAsync(c => c.Id == countryId);

				if (!validCountryId)
					return NotFound("CountryId does not match a stored record");

				if (stateId.HasValue)
				{
					bool validStateId = await _db.LStates.AnyAsync(s => s.Id == stateId);


					if (!validStateId)
						return NotFound("StateId does not match a stored record");
				}


				LocationCity newCityRecord = cityDto.GetDomainEntity(out bool successfulMapping, out IdentityError? validationError);

				_db.LCities.Add(newCityRecord);

				try
				{
					await _db.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Service is down");
				}

				cityDto.CityId = newCityRecord.Id;
				return Ok(cityDto);
			}
			else
				return BadRequest(ModelState.GetErrorList());

		}

		[HttpPatch("city")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditCity()
		{
			return Ok("Under Construction");
		}

		[HttpDelete("city")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteCity()
		{
			return Ok("Under Construction");
		}

	}
}
