using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TextAnalysis
{
	[Authorize(Roles = "admin,vip,registrated")]
	[Route("api")]
	[ApiController]
	public class TemporalSlangsApiController : ControllerBase
	{
		private readonly ISlangRepository slangRepository;
		private readonly ITemporalSingleWordsRepository tempSlanglsRepository;
		public TemporalSlangsApiController(ISlangRepository _slangRepository, ITemporalSingleWordsRepository _tempSlanglsRepository)
		{
			tempSlanglsRepository = _tempSlanglsRepository;
			slangRepository = _slangRepository;
		}

		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalSlangsCollectionName";
		private const string datatype = "Slang";

		[HttpGet("temp_slangs")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<TemporalObject> slangs = tempSlanglsRepository.GetAllWords(datacollection);
				return Ok(slangs);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("temp_slangs")]
		public IActionResult PostWord(FullText word)
		{
			if (word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempSlangs PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!slangRepository.IfWordExists(word.textToCheck) && !tempSlanglsRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject slang = tempSlanglsRepository.PostWord(datacollection, datatype, word.textToCheck);
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
				Debug.WriteLine("tempSlangs PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_slangs/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempSlangs PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!slangRepository.IfWordExists(word.textToCheck) && !tempSlanglsRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject slang = tempSlanglsRepository.PutWord(datacollection, datatype, word.textToCheck, connectionWord);
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
				Debug.WriteLine("tempSlangs PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		//[HttpDelete("temp_slangs/{wordToRemove}")]
		//public IActionResult DeleteWord(string wordToRemove)
		//{
		//	if (wordToRemove == null || wordToRemove.Equals(String.Empty))
		//	{
		//		Debug.WriteLine("tempSlangs DeleteWord: " + "Data is null.");
		//		return BadRequest("Data is null.");
		//	}
		//	try
		//	{
		//		int slangs = tempSlanglsRepository.DeleteWord(datacollection, wordToRemove);
		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		Errors errors = ErrorsHelper.GetErrors(ex);
		//		return StatusCode(StatusCodes.Status500InternalServerError, errors);
		//	}
		//}

		[HttpDelete("temp_slangs/{mongoId}")]
		public IActionResult DeleteWord(string mongoId)
		{
			if (mongoId == null || mongoId.Equals(String.Empty))
			{
				Debug.WriteLine("tempSlangs DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int slangs = tempSlanglsRepository.DeleteWord(datacollection, mongoId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("temp_slangs")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int deleted = tempSlanglsRepository.DeleteCollection(datacollection);
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