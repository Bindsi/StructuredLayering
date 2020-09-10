using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CosmosDbUtil.Model;
using System.Threading.Tasks;
using CosmosDbUtil.DataAccess;
using Microsoft.Extensions.Logging;

namespace CosmosDbUtil.BusinessLogic
{
	/// <inheritdoc />
	public class ProcessingLogic : IProcessingLogic
	{
		private readonly ICosmosDbAccessor _cosmosDbAccessor;

		public ProcessingLogic(ICosmosDbAccessor cosmosDbAccessor)
		{
			_cosmosDbAccessor = cosmosDbAccessor;
		}

		public async Task CreateDocumentsInBatchesAsync(ILogger logger)
		{
			try
			{
				var tasks = new List<Task>();
				for (int i = 0; i < 200; i++)
				{
					var startRange = i * 1000;
					var task = Task.Run(async () =>
					{
						var json = await File.ReadAllTextAsync(@".\Payload\Document.json");
						var document = JsonSerializer.Deserialize<DocumentDto>(json);
						for (int j = startRange + 1; j <= startRange + 1000; j++)
						{
							var partitionKey = j % 16;
							document.Id = Guid.NewGuid().ToString();
							document.PartitionKey = partitionKey.ToString();
							logger.LogInformation($"Upserting document with Id: {document.Id}, PartitionKey: {partitionKey}");
							var response = await _cosmosDbAccessor.UpsertItemAsync<DocumentDto>(logger, document, partitionKey.ToString());
						}
					});
					tasks.Add(task);
				}

				Task.WaitAll(tasks.ToArray());

			}
			catch (Exception e)
			{
				logger.LogError($"Error while creating documents - {e.Message}");
				throw;
			}
		}

		private async Task CreatePayload(ILogger logger, int startRange)
		{
			var json = await File.ReadAllTextAsync(@".\Payload\Document.json");
			var document = JsonSerializer.Deserialize<DocumentDto>(json);
			for (int j = startRange + 1; j <= startRange + 1000; j++)
			{
				var partitionKey = j % 16;
				document.Id = Guid.NewGuid().ToString();
				logger.LogInformation($"Upserting document with Id: {document.Id}, PartitionKey: {partitionKey}");
				var response = await _cosmosDbAccessor.UpsertItemAsync<DocumentDto>(logger, document, partitionKey.ToString());
			}
		}

		public async Task<IEnumerable<DocumentDto>> GetDocumentsAsync(ILogger logger)
		{
			try
			{
				logger.LogInformation($"Retrieving documents");
				return await _cosmosDbAccessor.GetDocumentsAsync<DocumentDto>(logger);
			}
			catch (Exception e)
			{
				logger.LogError($"Error while retrieving documents - {e.Message}");
				throw;
			}
		}

		public async Task UpdateDocumentsAsync(ILogger logger)
		{
			try
			{
				logger.LogInformation($"Retrieving documents");
				var documents = await _cosmosDbAccessor.GetDocumentsAsync<DocumentDto>(logger);
				foreach (var doc in documents)
				{
					doc.MyProperty = doc.MyProperty == "sampleProperty" ? "updatedProperty" : "sampleProperty";
					var response = await _cosmosDbAccessor.UpsertItemAsync(logger, doc, doc.PartitionKey);
					if (!response.IsSuccessStatusCode)
						continue;

					logger.LogInformation($"Request Charge for Id {doc.Id}: {response.Headers.RequestCharge}");
				}
			}
			catch (Exception e)
			{
				logger.LogError($"Error while updating documents - {e.Message}");
				throw;
			}
		}

	}
}