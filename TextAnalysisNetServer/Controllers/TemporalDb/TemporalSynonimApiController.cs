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
	public class TemporalSynonimApiController : ControllerBase
	{
		private readonly ISynonimRepository synonimRepository;
		private readonly ITemporalRelationalWordsRepository tempSynonimRepository;
		public TemporalSynonimApiController(ISynonimRepository _synonimRepository, ITemporalRelationalWordsRepository _tempSynonimRepository)
		{
			tempSynonimRepository = _tempSynonimRepository;
			synonimRepository = _synonimRepository;
		}

		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalSynonimsCollectionName";
		private const string datatype = "Synonim";

		[HttpGet("temp_synonims")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<TemporalObject> allSynonims = tempSynonimRepository.GetAllWords(datacollection);
				return Ok(allSynonims);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("temp_synonims")]
		public IActionResult PostWord(FullText synonimWord)
		{
			if (synonimWord == null || synonimWord.textToCheck == null || synonimWord.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempSynonim PostWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!synonimRepository.IfWordExists(synonimWord.textToCheck) && !tempSynonimRepository.IfWordExists(datacollection, synonimWord.textToCheck))
			{
				try
				{
					TemporalObject synonim = tempSynonimRepository.PostWord(datacollection, datatype, synonimWord.textToCheck);
					return StatusCode(StatusCodes.Status201Created, synonim);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("tempSynonim PostWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_synonims/{connectionWord}")]
		public IActionResult PutWord(FullText word, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word == null || word.textToCheck == null || word.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempSynonim PutWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!synonimRepository.IfWordExists(word.textToCheck) && !tempSynonimRepository.IfWordExists(datacollection, word.textToCheck))
			{
				try
				{
					TemporalObject synonim = tempSynonimRepository.PutWord(datacollection, datatype, word.textToCheck, connectionWord);
					return Ok(synonim);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("tempSynonim PutWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		[HttpPut("temp_synonims/insert/{connectionWord}")]
		public IActionResult InsertWord(FullText word_to_add, string connectionWord)
		{
			if (connectionWord == null || connectionWord.Equals(String.Empty) || word_to_add == null || word_to_add.textToCheck == null || word_to_add.textToCheck.Equals(String.Empty))
			{
				Debug.WriteLine("tempSynonim InsertWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			if (!synonimRepository.IfWordExists(word_to_add.textToCheck) && !tempSynonimRepository.IfWordExists(datacollection, word_to_add.textToCheck))
			{
				try
				{
					TemporalObject synonim = tempSynonimRepository.InsertWord(datacollection, datatype, word_to_add.textToCheck, connectionWord);
					return Ok(synonim);
				}
				catch (Exception ex)
				{
					Errors errors = ErrorsHelper.GetErrors(ex);
					return StatusCode(StatusCodes.Status500InternalServerError, errors);
				}
			}
			else
			{
				Debug.WriteLine("tempSynonim InsertWord: " + "Data allready exists.");
				return Conflict("Data allready exists.");
			}
		}

		//[HttpDelete("temp_synonims/{wordToRemove}")]
		//public IActionResult DeleteWord(string wordToRemove)
		//{
		//	if (wordToRemove == null || wordToRemove.Equals(String.Empty))
		//	{
		//		Debug.WriteLine("tempSynonim DeleteWord: " + "Data is null.");
		//		return BadRequest("Data is null.");
		//	}
		//	try
		//	{
		//		int synonims = tempSynonimRepository.DeleteWord(datacollection, wordToRemove);
		//		return NoContent();
		//	}
		//	catch (Exception ex)
		//	{
		//		Errors errors = ErrorsHelper.GetErrors(ex);
		//		return StatusCode(StatusCodes.Status500InternalServerError, errors);
		//	}
		//}

		[HttpDelete("temp_synonims/{mongoId}")]
		public IActionResult DeleteWord(string mongoId)
		{
			if (mongoId == null || mongoId.Equals(String.Empty))
			{
				Debug.WriteLine("tempSynonim DeleteWord: " + "Data is null.");
				return BadRequest("Data is null.");
			}
			try
			{
				int synonims = tempSynonimRepository.DeleteWord(datacollection, mongoId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("temp_synonims")]
		public IActionResult DeleteCollection()
		{
			try
			{
				int i = tempSynonimRepository.DeleteCollection(datacollection);
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
