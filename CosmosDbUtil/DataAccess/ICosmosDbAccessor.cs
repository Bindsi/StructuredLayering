using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace CosmosDbUtil.DataAccess
{
	/// <summary>
	/// Cosmos Db accessor
	/// </summary>
	public interface ICosmosDbAccessor
	{
		/// <summary>
		/// Upserts a document in Cosmos Db
		/// </summary>
		/// <typeparam name="T">Type of upserted document</typeparam>
		/// <param name="logger"></param>
		/// <param name="item">Payload</param>
		/// <param name="partitionKey">PartitionKey</param>
		/// <returns></returns>
		Task<ResponseMessage> UpsertItemAsync<T>(ILogger logger, T item, string partitionKey);

		/// <summary>
		/// Gets documents from Cosmos Db
		/// </summary>
		/// <typeparam name="T">Type of stored document</typeparam>
		/// <param name="logger"></param>
		/// <returns></returns>
		Task<IEnumerable<T>> GetDocumentsAsync<T>(ILogger logger);

	}
}