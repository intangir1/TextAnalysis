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
	public class TemporalAntonimApiController : ControllerBase
	{
		private readonly IAntonimRepository antonimRepository;
		private readonly ITemporalRelationalWordsRepository tempAntonimRepository;
		public TemporalAntonimApiController(IAntonimRepository _antonimRepository, ITemporalRelationalWordsRepository _tempAntonimRepository)
		{
			tempAntonimRepository = _tempAntonimRepository;
			antonimRepository = _antonimRepository;
		}

		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalAntonimsCollectionName";
		private const string datatype = "Antonim";

		[HttpGet("temp_antonims")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<TemporalObject> allAntonims = tempAntonimRepository.GetAllWords(datacollection);
				return Ok(allAntonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("temp_antonims/{connectionWord}")]
		public IActionResult PostWord(FullText antonimWord, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || antonimWord == null || antonimWord.textToCheck == null || antonimWord.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempAntonim PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!antonimRepository.IfWordExists(antonimWord.textToCheck) && !tempAntonimRepository.IfWordExists(datacollection, antonimWord.textToCheck))
			{
				try
				{
					TemporalObject antonim = tempAntonimRepository.PostWord(datacollection, datatype, antonimWord.textToCheck, connectionWord);
					return StatusCode(StatusCodes.Status201Created, antonim);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("tempAntonim PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_antonims/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempAntonim PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!antonimRepository.IfWordExists(word.textToCheck) && !tempAntonimRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject antonim = tempAntonimRepository.PutWord(datacollection, datatype, word.textToCheck, connectionWord);
					return Ok(antonim);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("tempAntonim PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_antonims/insert/{connectionWord}")]
		public IActionResult InsertWord(FullText word_to_add, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word_to_add == null || word_to_add.textToCheck == null || word_to_add.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempAntonim InsertWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!antonimRepository.IfWordExists(word_to_add.textToCheck) && !tempAntonimRepository.IfWordExists(datacollection, word_to_add.textToCheck))
			{
				try
				{
					TemporalObject antonim = tempAntonimRepository.InsertWord(datacollection, datatype, word_to_add.textToCheck, connectionWord);
					return Ok(antonim);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("tempAntonim InsertWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		//[HttpDelete("temp_antonims/{wordToRemove}")]
		//public IActionResult DeleteWord(string wordToRemove)
		//{
		//	if (wordToRemove == null || wordToRemove.Equals(String.Empty))
		//	{
		//		Debug.WriteLine("tempAntonim DeleteWord: " + "Data is null.");
		//		return BadRequest("Data is null.");
		//	}
		//	try
		//	{
		//		int antonims = tempAntonimRepository.DeleteWord(datacollection, wordToRemove);
		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		Errors errors = ErrorsHelper.GetErrors(ex);
		//		return StatusCode(StatusCodes.Status500InternalServerError, errors);
		//	}
		//}

		[HttpDelete("temp_antonims/{mongoId}")]
		public IActionResult DeleteWord(string mongoId)
		{
			if (mongoId == null || mongoId.Equals(String.Empty))
			{
				Debug.WriteLine("tempAntonim DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int antonims = tempAntonimRepository.DeleteWord(datacollection, mongoId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("temp_antonims")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int i = tempAntonimRepository.DeleteCollection(datacollection);
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
