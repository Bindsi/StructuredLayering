using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CosmosDbCommon;
using Microsoft.Azure.Cosmos;

namespace CosmosDbAdapter
{
	/// <inheritdoc />
	public class CosmosDbClient : ICosmosDbClient
	{
		private CosmosClient _cosmosClient;
		private Container _container;

		public CosmosDbClient(ICosmosDbConfiguration config)
		{
			_cosmosClient = new CosmosClient(config.CosmosDbUrl, config.CosmosDbKey,
				new CosmosClientOptions()
				{
					AllowBulkExecution = true
				});
			_container = _cosmosClient.GetContainer(config.DatabaseName, config.ContainerName);
		}

		public async Task<IEnumerable<T>> GetDocumentsAsync<T>(string query, string partitionKey)
		{
			var feedIterator =
				_container.GetItemQueryIterator<T>(
					new QueryDefinition(query),
					null,
					new QueryRequestOptions()
					{
						MaxItemCount = 1000,
						PartitionKey = new PartitionKey(partitionKey)
					});

			var documents = new List<T>();
			while (feedIterator.HasMoreResults)
			{
				FeedResponse<T> response = await feedIterator.ReadNextAsync();
				documents.AddRange(response);
			}

			return documents;
		}

		public async Task<ResponseMessage> UpsertDocumentAsync<T>(T payload, string partitionKey)
		{
			using (MemoryStream streamPayload = new MemoryStream())
			{
				await JsonSerializer.SerializeAsync(streamPayload, payload);
				return await _container.UpsertItemStreamAsync(streamPayload, new PartitionKey(partitionKey));
			}
		}
	}
}
