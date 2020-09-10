using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmosDbUtil.Model
{

	public class DocumentDto
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
		[JsonPropertyName("partitionKey")]
		public string PartitionKey { get; set; }
		[JsonPropertyName("myProperty")]
		public string MyProperty { get; set; }
	}

}