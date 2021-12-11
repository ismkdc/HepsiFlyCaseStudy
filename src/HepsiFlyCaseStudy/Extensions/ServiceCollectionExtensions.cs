using HepsiFlyCaseStudy.Models;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace HepsiFlyCaseStudy.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMongoDB(this IServiceCollection services)
    {
        services.AddSingleton<MongoDBContext>();
    }

    public static void AddRedis(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var configuration = provider.GetService<IConfiguration>();
        var conf = new RedisConfiguration
        {
            ConnectionString = configuration.GetConnectionString("Redis"),
            ServerEnumerationStrategy = new ServerEnumerationStrategy
            {
                Mode = ServerEnumerationStrategy.ModeOptions.All,
                TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
            }
        };

        services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(conf);
    }
}