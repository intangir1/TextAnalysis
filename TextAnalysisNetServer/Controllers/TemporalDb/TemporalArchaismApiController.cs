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
	public class TemporalArchaismApiController : ControllerBase
	{
		private readonly IArchaismRepository archaismRepository;
		private readonly ITemporalSingleWordsRepository tempArchaismRepository;
		public TemporalArchaismApiController(IArchaismRepository _archaismRepository, ITemporalSingleWordsRepository _tempArchaismRepository)
		{
			tempArchaismRepository = _tempArchaismRepository;
			archaismRepository = _archaismRepository;
		}

		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalArchaismsCollectionName";
		private const string datatype = "Archaism";

		[HttpGet("temp_archaisms")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<TemporalObject> archaisms = tempArchaismRepository.GetAllWords(datacollection);
				return Ok(archaisms);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("temp_archaisms")]
		public IActionResult PostWord(FullText word)
		{
			if (word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempArchaism PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!archaismRepository.IfWordExists(word.textToCheck) && !tempArchaismRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject archaism = tempArchaismRepository.PostWord(datacollection, datatype, word.textToCheck);
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
				Debug.WriteLine("tempArchaism PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_archaisms/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempArchaism PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!archaismRepository.IfWordExists(word.textToCheck) && !tempArchaismRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject archaism = tempArchaismRepository.PutWord(datacollection, datatype, word.textToCheck, connectionWord);
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
				Debug.WriteLine("tempArchaism PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		//[HttpDelete("temp_archaisms/{wordToRemove}")]
		//public IActionResult DeleteWord(string wordToRemove)
		//{
		//	if (wordToRemove == null || wordToRemove.Equals(String.Empty))
		//	{
		//		Debug.WriteLine("tempArchaism DeleteWord: " + "Data is null.");
		//		return BadRequest("Data is null.");
		//	}
		//	try
		//	{
		//		int archaisms = tempArchaismRepository.DeleteWord(datacollection, wordToRemove);
		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		Errors errors = ErrorsHelper.GetErrors(ex);
		//		return StatusCode(StatusCodes.Status500InternalServerError, errors);
		//	}
		//}

		[HttpDelete("temp_archaisms/{mongoId}")]
		public IActionResult DeleteWord(string mongoId)
		{
			if (mongoId == null || mongoId.Equals(String.Empty))
			{
				Debug.WriteLine("tempArchaism DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int archaisms = tempArchaismRepository.DeleteWord(datacollection, mongoId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("temp_archaisms")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int deleted = tempArchaismRepository.DeleteCollection(datacollection);
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