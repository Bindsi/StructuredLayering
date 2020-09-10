
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDbAdapter;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace CosmosDbUtil.DataAccess
{
	/// <inheritdoc />
	public class CosmosDbAccessor : ICosmosDbAccessor
	{
		private readonly ICosmosDbClient _cosmosDbClient;

		public CosmosDbAccessor(ICosmosDbClient cosmosDbClient)
		{
			_cosmosDbClient = cosmosDbClient;
		}

		public async Task<ResponseMessage> UpsertItemAsync<T>(ILogger logger, T item, string partitionKey)
		{
			try
			{
				logger.LogInformation("Upsert document");
				return await _cosmosDbClient.UpsertDocumentAsync(item, partitionKey);
			}
			catch (Exception e)
			{
				logger.LogError($"Error while upserting document - {e.Message}");
				throw;
			}
		}

		public async Task<IEnumerable<T>> GetDocumentsAsync<T>(ILogger logger)
		{
			try
			{
				var query = "Select * from documents";
				var partitionKey = new Random().Next(1, 16);
				logger.LogInformation("Retrieving documents");
				return await _cosmosDbClient.GetDocumentsAsync<T>(query, partitionKey.ToString());
			}
			catch (Exception e)
			{
				logger.LogError($"Error while retrieving documents - {e.Message}");
				throw;
			}
		}
	}
}