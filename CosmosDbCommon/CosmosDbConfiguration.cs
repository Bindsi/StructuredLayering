using System;

namespace CosmosDbCommon
{
	/// <inheritdoc />
	public class CosmosDbConfiguration : ICosmosDbConfiguration
	{
		public string CosmosDbUrl { get; set; }

		public string CosmosDbKey { get; set; }

		public string DatabaseName { get; set; }

		public string ContainerName { get; set; }
	}
}
