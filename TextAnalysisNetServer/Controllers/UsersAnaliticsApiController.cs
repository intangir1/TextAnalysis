using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TextAnalysis
{
	[Authorize(Roles = "admin")]
	[Route("api")]
	[ApiController]
	public class UsersAnaliticsApiController : ControllerBase
	{
		private IUserAnaliticsRepository userAnaliticsRepository;

		public UsersAnaliticsApiController(IUserAnaliticsRepository _userAnaliticsRepository)
		{
			userAnaliticsRepository = _userAnaliticsRepository;
		}

		[HttpGet("users_analitics")]
		public IActionResult GetAllUserAnalitics()
		{
			try
			{
				List<User> allUsersAnalitics = userAnaliticsRepository.GetAllUserAnalitics();
				return Ok(allUsersAnalitics);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpGet("users_analitics/{startDate}/{endDate}")]
		public IActionResult GetUserAnaliticsByDates(DateTime startDate, DateTime endDate)
		{
			try
			{
				List<User> allUsersAnalitics = userAnaliticsRepository.GetUserAnaliticsByDates(startDate, endDate);
				return Ok(allUsersAnalitics);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpGet("users_analitics/start/{startDate}")]
		public IActionResult GetUserAnaliticsByStart(DateTime startDate)
		{
			try
			{
				List<User> allUsersAnalitics = userAnaliticsRepository.GetUserAnaliticsByStart(startDate);
				return Ok(allUsersAnalitics);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpGet("users_analitics/end/{endDate}")]
		public IActionResult GetUserAnaliticsByEnd(DateTime endDate)
		{
			try
			{
				List<User> allUsersAnalitics = userAnaliticsRepository.GetUserAnaliticsByEnd(endDate);
				return Ok(allUsersAnalitics);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}
	}
}
