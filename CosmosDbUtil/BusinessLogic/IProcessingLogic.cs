using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDbUtil.Model;
using Microsoft.Extensions.Logging;

namespace CosmosDbUtil.BusinessLogic
{
	/// <summary>
	/// Processing logic for Cosmos Db documents
	/// </summary>
	public interface IProcessingLogic
	{
		/// <summary>
		/// Creates documents asynchronously in batches 
		/// </summary>
		/// <param name="logger">logger</param>
		/// <returns></returns>
		Task CreateDocumentsInBatchesAsync(ILogger logger);

		/// <summary>
		/// Gets stored documents from CosmosDb
		/// </summary>
		/// <typeparam name="T">Type of stored document</typeparam>
		/// <param name="logger">logger</param>
		/// <returns></returns>
		Task<IEnumerable<DocumentDto>> GetDocumentsAsync(ILogger logger);

		/// <summary>
		/// Updates documents in CosmosDb
		/// </summary>
		/// <param name="logger">logger</param>
		/// <returns></returns>
		Task UpdateDocumentsAsync(ILogger logger);
	}
}