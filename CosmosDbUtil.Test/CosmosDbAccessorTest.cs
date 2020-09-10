using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDbUtil.BusinessLogic;
using CosmosDbUtil.DataAccess;
using CosmosDbUtil.Model;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CosmosDbUtil.Test
{
	[TestClass]
	public class CosmosDbAccessorTest
	{
		private ICosmosDbAccessor _cosmosDbAccessor;
		private ILogger _logger;
		private List<DocumentDto> _documents;

		private IProcessingLogic _sut;

		[TestInitialize]
		public void Initialize()
		{

			_documents = new List<DocumentDto>();
			_documents.Add(new DocumentDto()
			{
				Id = Guid.NewGuid().ToString(),
				MyProperty = "sampleProperty",
				PartitionKey = "1"
			});
			_logger = NSubstitute.Substitute.For<ILogger>();
			_cosmosDbAccessor = NSubstitute.Substitute.For<ICosmosDbAccessor>();
			_cosmosDbAccessor.GetDocumentsAsync<DocumentDto>(Arg.Any<ILogger>()).ReturnsForAnyArgs(_documents);

			_sut = new ProcessingLogic(_cosmosDbAccessor);
		}

		[TestMethod]
		public async Task TestMethod1()
		{
			var documents = await _sut.GetDocumentsAsync(_logger);
			Assert.IsNotNull(documents);
			Assert.Equals(_documents.Count, documents.Count());
		}
	}
}
