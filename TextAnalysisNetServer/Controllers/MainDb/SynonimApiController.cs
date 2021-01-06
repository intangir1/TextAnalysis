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
	public class SynonimApiController : Controller
	{
		private readonly ISynonimRepository synonimRepository;
		public SynonimApiController(ISynonimRepository _synonimRepository)
		{
			synonimRepository = _synonimRepository;
		}

		[HttpGet("synonims")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<List<string>> allSynonims = synonimRepository.GetAllWords();
				return Ok(allSynonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpGet("synonims/{word}", Name = "GetSynonimsBy")]
		public IActionResult GetWordsBy(string word)
		{
			try
			{
				List<string> synonims = synonimRepository.GetWordsBy(word);
				return Ok(synonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("synonims")]
		public IActionResult PostWord(FullText word)
		{
			if (word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("synonim PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!synonimRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					List<string> synonims = synonimRepository.PostWord(word.textToCheck);
					return StatusCode(StatusCodes.Status201Created, synonims);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("synonim PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("synonims/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("synonim PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!synonimRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					List<string> synonims = synonimRepository.PutWord(word.textToCheck, connectionWord);
					return Ok(synonims);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("synonim PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("synonims/insert/{connectionWord}")]
		public IActionResult InsertWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("synonim InsertWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!synonimRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					List<string> synonims = synonimRepository.InsertWord(word.textToCheck, connectionWord);
					return Ok(synonims);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("synonim InsertWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("synonims/{wordToRemove}")]
		public IActionResult DeleteWord(string wordToRemove)
		{
			if (wordToRemove == null || wordToRemove.Equals(String.Empty))
			{
				Debug.WriteLine("synonim DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				List<string> synonims = synonimRepository.DeleteWord(wordToRemove);
				return Ok(synonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("synonims/delete_collection/{word}")]
		public IActionResult DeleteCollection(string word)
		{
			if (word == null || word.Equals(String.Empty))
			{
				Debug.WriteLine("synonim DeleteCollection: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int i = synonimRepository.DeleteCollection(word);
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
