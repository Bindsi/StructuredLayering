using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CosmosDbAdapter
{
	/// <summary>
	/// CosmosDbClient
	/// </summary>
	public interface ICosmosDbClient
	{
		/// <summary>
		/// Gets documents from Cosmos Db
		/// </summary>
		/// <typeparam name="T">Type of stored document</typeparam>
		/// <param name="query">Query</param>
		/// <param name="partitionKey">PartitionKey</param>
		/// <returns></returns>
		Task<IEnumerable<T>> GetDocumentsAsync<T>(string query, string partitionKey);

		/// <summary>
		/// Upserts a document in Cosmos Db
		/// </summary>
		/// <typeparam name="T">Type of upserted document</typeparam>
		/// <param name="payload">Payload</param>
		/// <param name="partitionKey">PartitionKey</param>
		/// <returns></returns>
		Task<ResponseMessage> UpsertDocumentAsync<T>(T payload, string partitionKey);
	}
}