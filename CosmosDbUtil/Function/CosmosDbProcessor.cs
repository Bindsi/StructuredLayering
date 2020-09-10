using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CosmosDbUtil.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CosmosDbUtil.Function
{
	public class CosmosDbProcessor
	{
		private readonly IProcessingLogic _processLogic;

		public CosmosDbProcessor(IProcessingLogic processLogic)
		{
			_processLogic = processLogic;
		}

		[FunctionName("CreateCosmosDbDocs")]
		public async Task<IActionResult> CreateCosmosDbDocs(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "cosmosDbProcessing/createCosmosDbDocs")] HttpRequest req,
			ILogger log)
		{
			try
			{
				await _processLogic.CreateDocumentsInBatchesAsync(log);
				return (ActionResult)new OkObjectResult($"Succeed");
			}
			catch (CosmosException ce)
			{
				return new ObjectResult(new { StatusCode = 500, ErrorMessage = ce.Message });
			}
			catch (Exception e)
			{
				return new BadRequestObjectResult(e.Message);
			}

		}

		[FunctionName("UpdateCosmosDbDocs")]
		public async Task UpdateCosmosDbDocs([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer, ILogger log)
		{
			try
			{
				await _processLogic.UpdateDocumentsAsync(log);
			}
			catch (CosmosException ce)
			{
				log.LogError("Error: - " + ce.Message);
			}
			catch (Exception e)
			{
				log.LogError("Error: - " + e.Message);
			}
		}

		[FunctionName("GetCosmosDbDocs")]
		public async Task<IActionResult> GetCosmosDbDocs(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "costEvaluation/getCosmosDbDocs")]
			HttpRequest req,
			ILogger log)
		{
			try
			{
				var documents = _processLogic.GetDocumentsAsync(log);

				return (ActionResult)new OkObjectResult(documents);
			}
			catch (CosmosException ce)
			{
				return new ObjectResult(new { StatusCode = 500, ErrorMessage = ce.Message });
			}
			catch (Exception e)
			{
				return new BadRequestObjectResult(e.Message);
			}
		}


	}

}
