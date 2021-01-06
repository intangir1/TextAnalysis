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
	public class ExpressionsApiController : ControllerBase
	{
		private readonly IExpressionsRepository expressionsRepository;
		public ExpressionsApiController(IExpressionsRepository _expressionsRepository)
		{
			expressionsRepository = _expressionsRepository;
		}

		[HttpGet("expressions")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<string> expressions = expressionsRepository.GetAllWords();
				return Ok(expressions);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("expressions")]
		public IActionResult PostWord(FullText word)
		{
			if (word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("expression PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!expressionsRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					string expression = expressionsRepository.PostWord(word.textToCheck);
					return StatusCode(StatusCodes.Status201Created, expression);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("expression PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("expressions/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("expression PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!expressionsRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					string expression = expressionsRepository.PutWord(word.textToCheck, connectionWord);
					return Ok(expression);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("expression PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("expressions/{wordToRemove}")]
		public IActionResult DeleteWord(string wordToRemove)
		{
			if (wordToRemove == null || wordToRemove.Equals(String.Empty))
			{
				Debug.WriteLine("expression DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int expression = expressionsRepository.DeleteWord(wordToRemove);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("expressions")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int deleted = expressionsRepository.DeleteCollection();
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
