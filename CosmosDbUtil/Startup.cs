using System;
using CosmosDbAdapter;
using CosmosDbCommon;
using CosmosDbUtil.BusinessLogic;
using CosmosDbUtil.DataAccess;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CosmosDbUtil.Startup))]

namespace CosmosDbUtil
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			var config = new CosmosDbConfiguration()
			{
				ContainerName = Environment.GetEnvironmentVariable("ContainerName"),
				CosmosDbKey = Environment.GetEnvironmentVariable("CosmosDbKey"),
				CosmosDbUrl = Environment.GetEnvironmentVariable("CosmosDbUrl"),
				DatabaseName = Environment.GetEnvironmentVariable("DatabaseName")
			};
			builder.Services.AddSingleton<ICosmosDbConfiguration>(config);
			builder.Services.AddSingleton<ICosmosDbClient, CosmosDbClient>();
			builder.Services.AddScoped<IProcessingLogic, ProcessingLogic>();
			builder.Services.AddScoped<ICosmosDbAccessor, CosmosDbAccessor>();
		}

	}
}