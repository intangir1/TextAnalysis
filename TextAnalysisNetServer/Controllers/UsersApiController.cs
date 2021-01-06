using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TextAnalysis
{
	[Route("api")]
	[ApiController]
	public class UsersApiController : ControllerBase
	{
		private IUserRepository userRepository;
		private readonly IHostingEnvironment environment;

		public UsersApiController(IUserRepository _userRepository, IHostingEnvironment _environment)
		{
			userRepository = _userRepository;
			environment = _environment;
		}

		[Authorize(Roles = "admin")]
		[HttpGet("users")]
		public IActionResult GetAllUsers()
		{
			try
			{
				List<User> allUsers = userRepository.GetAllUsers();
				return Ok(allUsers);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpGet("users/{id}", Name = "GetOneUser")]
		public IActionResult GetOneUser(string id)
		{
			try
			{
				User oneUser = userRepository.GetOneUserById(id);
				return Ok(oneUser);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("users/check")]
		public IActionResult ReturnUserByNamePassword(Login loginModel)
		{
			try
			{
				if (loginModel == null)
				{
					return BadRequest("Data is null.");
				}
				if (!ModelState.IsValid)
				{
					Errors errors = ErrorsHelper.GetErrors(ModelState);
					return BadRequest(errors);
				}

				Login checkUser = userRepository.ReturnUserByNamePassword(loginModel);
				if (checkUser == null)
				{
					Errors errors = ErrorsHelper.GetErrors("Wrong username or password");
					return StatusCode(StatusCodes.Status401Unauthorized, errors);
				}
				return StatusCode(StatusCodes.Status201Created, checkUser);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[AllowAnonymous]
		[HttpPost("users")]
		public IActionResult AddUser(User userModel)
		{
			try
			{
				if (userModel == null)
				{
					return BadRequest("Data is null.");
				}
				if (!ModelState.IsValid)
				{
					Errors errors = ErrorsHelper.GetErrors(ModelState);
					return BadRequest(errors);
				}

				if (userRepository.GetOneUserById(userModel.userID) != null)
				{
					Debug.WriteLine("User AddUser: " + "User with this id allready exists.");
					return Conflict("User with this id allready exists.");
				}
				if (userModel.userImage!=null && !userModel.userImage.Equals("") && !userModel.userImage.Equals(String.Empty))
				{
					byte[] bytes = Convert.FromBase64String(userModel.userImage);
					string[] extensions = userModel.userPicture.Split('.');
					string extension = extensions[extensions.Length - 1];
					string fileName = Guid.NewGuid().ToString();
					string filePath = environment.WebRootPath + "/assets/images/users/" + fileName + "." + extension;
					using (FileStream binaryFileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
					{
						binaryFileStream.Write(bytes, 0, bytes.Length);
						userModel.userPicture = fileName + "." + extension;
					}
				}
				userModel.userImage = string.Empty;
				User addedUser = userRepository.AddUser(userModel);
				return StatusCode(StatusCodes.Status201Created, addedUser);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin,vip,registrated")]
		[HttpPut("users/{id}")]
		public IActionResult UpdateUser(string id, User userModel)
		{
			try
			{
				if (userModel == null)
				{
					return BadRequest("Data is null.");
				}
				if (!ModelState.IsValid)
				{
					Errors errors = ErrorsHelper.GetErrors(ModelState);
					return BadRequest(errors);
				}

				userModel.userID = id;
				User updatedUser = userRepository.UpdateUser(userModel);
				return Ok(updatedUser);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("users/{id}")]
		public IActionResult DeleteUser(string id)
		{
			try
			{
				int i = userRepository.DeleteUser(id, environment.WebRootPath + "/assets/images/users/");
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("users")]
		public IActionResult DeleteUsers()
		{
			try
			{
				int i = userRepository.DeleteUsers(environment.WebRootPath + "/assets/images/users/");
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}
	}
}