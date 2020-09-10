namespace CosmosDbCommon
{
	/// <summary>
	/// Configuration for Cosmos Db
	/// </summary>
	public interface ICosmosDbConfiguration
	{
		/// <summary>
		/// Cosmos Db url
		/// </summary>
		string CosmosDbUrl { get; set; }

		/// <summary>
		/// Cosmos Db key
		/// </summary>
		string CosmosDbKey { get; set; }

		/// <summary>
		/// Cosmos Db database name
		/// </summary>
		string DatabaseName { get; set; }

		/// <summary>
		/// Cosmos Db container name
		/// </summary>
		string ContainerName { get; set; }
	}
}