using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace TextAnalysis
{
	[Route("api")]
	[ApiController]
	[Authorize(Roles = "admin")]
	public class WordsApiController : ControllerBase
	{
		private readonly IWordsRepository wordsRepository;
		private readonly IHostingEnvironment environment;
		private readonly string filePath;

		public WordsApiController(IWordsRepository _wordsRepository, IHostingEnvironment _environment)
		{
			wordsRepository = _wordsRepository;
			environment = _environment;
			filePath = environment.WebRootPath;
			filePath = Path.GetFullPath(Path.Combine(filePath, @"..\..\"));
		}

		[HttpPost("addSimpleWords")]
		public IActionResult AddSimpleWords()
		{
			try
			{
				AnalysedText analysedText = wordsRepository.AddSimpleWords(filePath);
				return Ok(analysedText);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("addIrregularWords")]
		public IActionResult AddIrregulars()
		{
			try
			{
				List<IrregularObject> irregulars = wordsRepository.AddIrregulars(filePath);
				return Ok(irregulars);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("addRelationalWords")]
		public IActionResult AddRelationalWords()
		{
			try
			{
				List<List<string>> relationalWords = wordsRepository.AddRelationalWords(filePath);
				return Ok(relationalWords);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("deleteWords")]
		public IActionResult DeleteAllWords()
		{
			try
			{
				int deleted = wordsRepository.DeleteAllWords();
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("addTemporalSimpleWords")]
		public IActionResult AddTemporalSimpleWords()
		{
			try
			{
				List<TemporalObject> temporalObject = wordsRepository.AddTemporalSimpleWords(filePath);
				return Ok(temporalObject);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("addTemporalIrregularWords")]
		public IActionResult AddTemporalIrregulars()
		{
			try
			{
				List<TemporalObjectForIrregular> temporalIrregularObject = wordsRepository.AddTemporalIrregulars(filePath);
				return Ok(temporalIrregularObject);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("addTemporalRelationalWords")]
		public IActionResult AddTemporalRelationalWords()
		{
			try
			{
				List<TemporalObject> temporalRelationalWords = wordsRepository.AddTemporalRelationalWords(filePath);
				return Ok(temporalRelationalWords);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpDelete("deleteTemporalWords")]
		public IActionResult DeleteAllTemporalWords()
		{
			try
			{
				int deleted = wordsRepository.DeleteAllTemporalWords();
				return NoContent();
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpGet("getAllTemporalWords")]
		public IActionResult GetAllTemporalWords()
		{
			try
			{
				List<object> temporalObjects = wordsRepository.GetAllTemporalWords();
				return Ok(temporalObjects);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpGet("getAllWords")]
		public IActionResult GetAllWords()
		{
			try
			{
				List<object> temporalObjects = wordsRepository.GetAllWords();
				return Ok(temporalObjects);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}
	}
}
