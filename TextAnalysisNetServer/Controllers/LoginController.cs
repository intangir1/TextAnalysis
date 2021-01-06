using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace TextAnalysis
{
	[ApiController]
	public class LoginController : ControllerBase
	{
		private IUserRepository userRepository;
		private IUserAnaliticsRepository userAnaliticsRepository;

		public LoginController(IUserRepository _userRepository, IUserAnaliticsRepository _userAnaliticsRepository)
		{
			userRepository = _userRepository;
			userAnaliticsRepository = _userAnaliticsRepository;
		}

		[AllowAnonymous]
		[HttpPost("token")]
		public IActionResult Authenticate(Login login)
		{
			if (login == null)
			{
				Debug.WriteLine("token Authenticate: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			User user = userRepository.Authenticate(login.userNickName, login.userPassword);

			if (user == null)
				return BadRequest(new { message = "Name or Password is incorrect" });
			User oneUser = userRepository.GetOneUserById(user.userID);
			UserForStatistics userForStatistics = new UserForStatistics(oneUser.userID, oneUser.userRegistrationDate, DateTime.Now);
			userAnaliticsRepository.AddUserToLoginAnalitics(userForStatistics);
			user.userID = null;
			return Ok(user);
		}
	}
}