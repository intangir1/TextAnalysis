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
	public class TemporalExpressionsApiController : ControllerBase
	{
		private readonly IExpressionsRepository expressionsRepository;
		private readonly ITemporalSingleWordsRepository tempExpressionsRepository;
		public TemporalExpressionsApiController(IExpressionsRepository _expressionsRepository, ITemporalSingleWordsRepository _tempExpressionsRepository)
		{
			tempExpressionsRepository = _tempExpressionsRepository;
			expressionsRepository = _expressionsRepository;
		}

		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalEstablishedExpressionsCollectionName";
		private const string datatype = "Expression";

		[HttpGet("temp_expressions")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<TemporalObject> expressions = tempExpressionsRepository.GetAllWords(datacollection);
				return Ok(expressions);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("temp_expressions")]
		public IActionResult PostWord(FullText word)
		{
			if (word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempExpression PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!expressionsRepository.IfWordExists(word.textToCheck) && !tempExpressionsRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject expression = tempExpressionsRepository.PostWord(datacollection, datatype, word.textToCheck);
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
				Debug.WriteLine("tempExpression PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_expressions/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempExpression PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!expressionsRepository.IfWordExists(word.textToCheck) && !tempExpressionsRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject expression = tempExpressionsRepository.PutWord(datacollection, datatype, word.textToCheck, connectionWord);
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
				Debug.WriteLine("tempExpression PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		//[HttpDelete("temp_expressions/{wordToRemove}")]
		//public IActionResult DeleteWord(string wordToRemove)
		//{
		//	if (wordToRemove == null || wordToRemove.Equals(String.Empty))
		//	{
		//		Debug.WriteLine("tempExpression DeleteWord: " + "Data is null.");
		//		return BadRequest("Data is null.");
		//	}
		//	try
		//	{
		//		int expressions = tempExpressionsRepository.DeleteWord(datacollection, wordToRemove);
		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		Errors errors = ErrorsHelper.GetErrors(ex);
		//		return StatusCode(StatusCodes.Status500InternalServerError, errors);
		//	}
		//}

		[HttpDelete("temp_expressions/{mongoId}")]
		public IActionResult DeleteWord(string mongoId)
		{
			if (mongoId == null || mongoId.Equals(String.Empty))
			{
				Debug.WriteLine("tempExpression DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int expressions = tempExpressionsRepository.DeleteWord(datacollection, mongoId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("temp_expressions")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int deleted = tempExpressionsRepository.DeleteCollection(datacollection);
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