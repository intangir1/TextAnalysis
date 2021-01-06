using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TextAnalysis
{
	[Route("api")]
	[ApiController]
	[Authorize(Roles = "admin,vip,registrated")]
	public class SlangsApiController : ControllerBase
	{
		private readonly ISlangRepository slangRepository;
		public SlangsApiController(ISlangRepository _slangRepository)
		{
			slangRepository = _slangRepository;
		}

		[HttpGet("slangs")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<string> slangs = slangRepository.GetAllWords();
				return Ok(slangs);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("slangs")]
		public IActionResult PostWord(FullText word)
		{
			if (word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("slangs PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!slangRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					string slang = slangRepository.PostWord(word.textToCheck);
					return StatusCode(StatusCodes.Status201Created, slang);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("slang PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("slangs/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("slangs PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!slangRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					string slang = slangRepository.PutWord(word.textToCheck, connectionWord);
					return Ok(slang);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("slang PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("slangs/{wordToRemove}")]
		public IActionResult DeleteWord(string wordToRemove)
		{
			if (wordToRemove == null || wordToRemove.Equals(String.Empty))
			{
				Debug.WriteLine("slangs DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int slangs = slangRepository.DeleteWord(wordToRemove);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("slangs")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int i = slangRepository.DeleteCollection();
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
