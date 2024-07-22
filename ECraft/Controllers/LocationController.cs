using ECraft.Constants;
using ECraft.Contracts.LocationDto;
using ECraft.Data;
using ECraft.Domain;
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
		private readonly ILogger<LocationController> _logger;

		public LocationController(AppDbContext db, ILogger<LocationController> logger)
		{
			_db = db;
			_logger = logger;
		}

		[HttpGet("country/{countryId}")]
		public async Task<IActionResult> GetCountry(int countryId)
		{
			LocationCountry countryWithCities = await _db.LCountries.Where(c => c.Id == countryId)
				.Include(c => c.CountryCities)
				.Include(c => c.CountryRegions)
				.FirstOrDefaultAsync();

			if (countryWithCities is null)
				return NotFound("Invalid CountryId");

			CountryDto countryDto = new CountryWithCitiesDto()
			{
				CountryRegions = countryWithCities.CountryRegions.ToList().Select(s => new RegionDto().GetDto(s)),
				CountryCities = countryWithCities.CountryCities.ToList().Select(c => new CityDto().GetDto(c)),
				citiesCount=countryWithCities.CountryCities.Count,
				statesCount=countryWithCities.CountryRegions.Count
			}.GetDto(countryWithCities);


			return Ok(countryDto);
		}

		[HttpGet("region/{RegionId}")]
		public async Task<IActionResult> GetRegion(int RegionId)
		{
			LocationRegion regionWithCities = await _db.LRegions.Where(s => s.Id == RegionId)
				.Include(s => s.RegionCities)
				.FirstOrDefaultAsync();

			if (regionWithCities is null)
				return NotFound("Invalid RegionId");

			RegionDto regionDto = new RegionWithCitiesDto()
			{
				RegionCities = regionWithCities.RegionCities.ToList().Select(c => new CityDto().GetDto(c)),
				CitiesCount=regionWithCities.RegionCities.Count
			}.GetDto(regionWithCities);


			return Ok(regionDto);
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
				LocationCountry newCountryRecord = countryDto.GetDomainEntity();

				_db.LCountries.Add(newCountryRecord);

				try
				{
					await _db.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					var errors = new ErrorList();
					errors.AddError(GeneralErrorCodes.ServiceDown, ex.Message);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
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

		[HttpPost("region")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddRegion([FromBody] RegionDto regionDto)
		{
			if (ModelState.IsValid)
			{
				int countryId = regionDto.CountryId;

				bool validCountryId = await _db.LCountries.AnyAsync(c => c.Id == countryId);

				if (!validCountryId)
					return NotFound("CountryId does not match a stored record");

				

				LocationRegion newStateRecord = regionDto.GetDomainEntity();

				_db.LRegions.Add(newStateRecord);

				try
				{
					await _db.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					return this.ReturnServerDownError(ex, _logger);
				}

				regionDto.RegionId = newStateRecord.Id;
				return Ok(regionDto);
			}
			else
				return BadRequest(ModelState.GetErrorList());
		}

		[HttpPatch("region")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditRegion()
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
				int? RegionId = cityDto.RegionId;


				bool validCountryId = await _db.LCountries.AnyAsync(c => c.Id == countryId);

				if (!validCountryId)
					return NotFound("CountryId does not match a stored record");

				if (RegionId.HasValue)
				{
					bool validRegionId = await _db.LRegions.AnyAsync(s => s.Id == RegionId);


					if (!validRegionId)
						return NotFound("RegionId does not match a stored record");
				}


				LocationCity newCityRecord = cityDto.GetDomainEntity();

				_db.LCities.Add(newCityRecord);

				try
				{
					await _db.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					return this.ReturnServerDownError(ex, _logger);
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
