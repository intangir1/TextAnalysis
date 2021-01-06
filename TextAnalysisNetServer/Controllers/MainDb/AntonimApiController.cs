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
	public class AntonimApiController : ControllerBase
	{
		private readonly IAntonimRepository antonimRepository;
		public AntonimApiController(IAntonimRepository _antonimRepository)
		{
			antonimRepository = _antonimRepository;
		}

		[HttpGet("antonims")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<List<string>> allAntonims = antonimRepository.GetAllWords();
				return Ok(allAntonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpGet("antonims/{word}", Name = "GetAntonimsBy")]
		public IActionResult GetWordsBy(string word)
		{
			try
			{
				List<string> antonims = antonimRepository.GetWordsBy(word);
				return Ok(antonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("antonims/{connectionWord}")]
		public IActionResult PostWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("antonim PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!antonimRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					List<string> antonims = antonimRepository.PostWord(word.textToCheck, connectionWord);
					return StatusCode(StatusCodes.Status201Created, antonims);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("antonim PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPost("antonims/{connectionWord}")]
		public IActionResult PostCollection(List<string> words, string connectionWord)
		{
			if (words == null || words.Count == 0 || words.Equals(String.Empty))
			{
				Debug.WriteLine("antonim PostCollection: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				List<string> synonims = antonimRepository.PostCollection(words, connectionWord);
				return StatusCode(StatusCodes.Status201Created, synonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("antonims/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("antonim PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!antonimRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					List<string> antonims = antonimRepository.PutWord(word.textToCheck, connectionWord);
					return Ok(antonims);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("antonim PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpPut("antonims/insert/{connectionWord}")]
		public IActionResult InsertWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("antonim InsertWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!antonimRepository.IfWordExists(word.textToCheck))
			{
				try
				{
					List<string> antonims = antonimRepository.InsertWord(word.textToCheck, connectionWord);
					return Ok(antonims);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("antonim InsertWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("antonims/{wordToRemove}")]
		public IActionResult DeleteWord(string wordToRemove)
		{
			if (wordToRemove == null || wordToRemove.Equals(String.Empty))
			{
				Debug.WriteLine("antonim DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				List<string> antonims = antonimRepository.DeleteWord(wordToRemove);
				return Ok(antonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[Authorize(Roles = "admin")]
		[HttpDelete("antonims/delete_collection/{word}")]
		public IActionResult DeleteCollection(string word)
		{
			if (word == null || word.Equals(String.Empty))
			{
				Debug.WriteLine("antonim DeleteCollection: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int i = antonimRepository.DeleteCollection(word);
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
