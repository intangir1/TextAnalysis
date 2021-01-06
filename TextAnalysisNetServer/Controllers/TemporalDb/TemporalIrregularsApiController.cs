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
	public class TemporalIrregularsApiController : ControllerBase
	{
		private readonly IIrregularVerbsRepository irregularVerbsRepository;
		private readonly ITemporalIrregularRepository tempIrregularsRepository;
		public TemporalIrregularsApiController(IIrregularVerbsRepository _irregularVerbsRepository, ITemporalIrregularRepository _tempIrregularsRepository)
		{
			tempIrregularsRepository = _tempIrregularsRepository;
			irregularVerbsRepository = _irregularVerbsRepository;
		}

		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalIrregularsCollectionName";

		[HttpGet("temp_irregulars")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<TemporalObjectForIrregular> irregulars = tempIrregularsRepository.GetAllWords(datacollection);
				return Ok(irregulars);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("temp_irregulars")]
		public IActionResult PostWord(IrregularObject word)
		{
			if (word == null || word.Equals(String.Empty))
			{
				Debug.WriteLine("tempIrregulars PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!irregularVerbsRepository.IfWordExists(word) && !tempIrregularsRepository.IfWordExists(datacollection, word))
			{
				try
				{
					TemporalObjectForIrregular irregular = tempIrregularsRepository.PostWord(datacollection, word);
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
				Debug.WriteLine("tempIrregulars PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_irregulars/{connectionWord}")]
		public IActionResult PutWord(IrregularObject word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.Equals(String.Empty))
			{
				Debug.WriteLine("tempIrregulars PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!irregularVerbsRepository.IfWordExists(word) && !tempIrregularsRepository.IfWordExists(datacollection, word))
			{
				try
				{
					TemporalObjectForIrregular irregular = tempIrregularsRepository.PutWord(datacollection, word, connectionWord);
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
				Debug.WriteLine("tempIrregulars PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		//[HttpDelete("temp_irregulars/{wordToRemove}")]
		//public IActionResult DeleteWord(string wordToRemove)
		//{
		//	if (wordToRemove == null || wordToRemove.Equals(String.Empty))
		//	{
		//		Debug.WriteLine("tempIrregulars DeleteWord: " + "Data is null.");
		//		return BadRequest("Data is null.");
		//	}
		//	try
		//	{
		//		int irregulars = tempIrregularsRepository.DeleteWord(datacollection, wordToRemove);
		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		Errors errors = ErrorsHelper.GetErrors(ex);
		//		return StatusCode(StatusCodes.Status500InternalServerError, errors);
		//	}
		//}

		[HttpDelete("temp_irregulars/{mongoId}")]
		public IActionResult DeleteWord(string mongoId)
		{
			if (mongoId == null || mongoId.Equals(String.Empty))
			{
				Debug.WriteLine("tempIrregulars DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int irregulars = tempIrregularsRepository.DeleteWord(datacollection, mongoId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("temp_irregulars")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int deleted = tempIrregularsRepository.DeleteCollection(datacollection);
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
