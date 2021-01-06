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
	public class ArchaismApiController : ControllerBase
	{
		private readonly IArchaismRepository archaismRepository;
		public ArchaismApiController(IArchaismRepository _archaismRepository)
		{
			archaismRepository = _archaismRepository;
		}

		[HttpGet("archaisms")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<string> archaisms = archaismRepository.GetAllWords();
				return Ok(archaisms);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("archaisms")]
		public IActionResult PostWord(FullText word)
		{
			if (word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("archaism PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!archaismRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					string archaism = archaismRepository.PostWord(word.textToCheck);
					return StatusCode(StatusCodes.Status201Created, archaism);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("archaism PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("archaisms/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("archaism PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!archaismRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					string archaism = archaismRepository.PutWord(word.textToCheck, connectionWord);
					return Ok(archaism);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("archaism PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("archaisms/{wordToRemove}")]
		public IActionResult DeleteWord(string wordToRemove)
		{
			if (wordToRemove == null || wordToRemove.Equals(String.Empty))
			{
				Debug.WriteLine("archaism DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int archaisms = archaismRepository.DeleteWord(wordToRemove);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("archaisms")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int deleted = archaismRepository.DeleteCollection();
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