using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace TextAnalysis
{
	//[Authorize(Roles = "admin,vip,registrated")]
	//[Authorize]
	[Route("api")]
	[ApiController]
	public class FullAnalyticsApiController : ControllerBase
	{
		private readonly IFullAnalyticsRepository fullAnalyticsRepository;
		public FullAnalyticsApiController(IFullAnalyticsRepository _fullAnalyticsRepository)
		{
			fullAnalyticsRepository = _fullAnalyticsRepository;
		}

		[HttpPost("AnalyseFullText/{limit}")]
		public IActionResult AnalyseFullText(FullText fullText, int limit)
		{
			var frontType = Request.Headers["frontType"];
			string textForDebug = "";
			string textForBadRequest = "";
			if (fullText == null || fullText.textToCheck == null || fullText.textToCheck.Equals(String.Empty))
			{
				textForDebug = textForDebug + "byWords PostWords Text: " + "Text is null.";
				textForBadRequest = textForBadRequest + "There is no text.";
			}
			if (limit == 0)
			{
				textForDebug = textForDebug + "byWords PostWords Limit: " + "limit is 0.";
				textForBadRequest = textForBadRequest + "There is no words limit.";
			}
			if (!textForDebug.Equals("") || !textForBadRequest.Equals(""))
			{
				Debug.WriteLine(textForDebug);
				return BadRequest(textForBadRequest);
			}
			try
			{
				string analysedText = fullAnalyticsRepository.AnalyseFullText(fullText.textToCheck, limit, frontType);
				return Ok(analysedText);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}

		[HttpPost("allSentencesWords")]
		public IActionResult CompareAllSentencesWords(string[] text)
		{
			var frontType = Request.Headers["frontType"];
			if (text == null || text.Length == 0)
			{
				Debug.WriteLine("CompareAllSentencesWords PostWords: " + "Data is null.");
				return BadRequest("There is no text.");
			}
			try
			{
				string analysedText = fullAnalyticsRepository.CompareAllSentencesWords(text, frontType);
				return Ok(analysedText);
			}
			catch (Exception ex)
			{
				Errors errors = ErrorsHelper.GetErrors(ex);
				return StatusCode(StatusCodes.Status500InternalServerError, errors);
			}
		}
	}
}
