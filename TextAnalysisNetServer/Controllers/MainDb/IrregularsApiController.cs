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
	public class IrregularsApiController : ControllerBase
	{
		private readonly IIrregularVerbsRepository irregularVerbsRepository;
		public IrregularsApiController(IIrregularVerbsRepository _irregularVerbsRepository)
		{
			irregularVerbsRepository = _irregularVerbsRepository;
		}

		[HttpGet("irregulars")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<IrregularObject> irregulars = irregularVerbsRepository.GetAllData();
				return Ok(irregulars);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("irregulars")]
		public IActionResult PostWord(IrregularObject word)
		{
			if (word == null || word.Equals(String.Empty))
			{
				Debug.WriteLine("irregulars PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!irregularVerbsRepository.IfWordExists(word))
			{
				try
				{
					IrregularObject irregular = irregularVerbsRepository.PostData(word);
					return StatusCode(StatusCodes.Status201Created, irregular);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("irregulars PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("irregulars/{connectionWord}")]
		public IActionResult PutWord(IrregularObject word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.Equals(String.Empty))
			{
				Debug.WriteLine("irregulars PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!irregularVerbsRepository.IfWordExists(word))
			{
				try
				{
					IrregularObject irregular = irregularVerbsRepository.PutData(word, connectionWord);
					return Ok(irregular);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("irregulars PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("irregulars/{wordToRemove}")]
		public IActionResult DeleteWord(string wordToRemove)
		{
			if (wordToRemove == null || wordToRemove.Equals(String.Empty))
			{
				Debug.WriteLine("irregulars DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int irregulars = irregularVerbsRepository.DeleteData(wordToRemove);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("irregulars")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int i = irregularVerbsRepository.DeleteCollection();
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
