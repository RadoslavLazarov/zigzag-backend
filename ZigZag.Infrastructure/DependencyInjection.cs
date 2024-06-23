using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Configurations;
using ZigZag.Domain.Configurations.UsersConfiguration;
using ZigZag.Domain.Entities.Identity;
using ZigZag.Domain.Entities.Venue;
using ZigZag.Infrastructure.Identity;
using ZigZag.Infrastructure.Middlewares;
using ZigZag.Infrastructure.Repositories;
using ZigZag.Infrastructure.Services;

namespace ZigZag.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDbConfig:ConnectionString").Value;
            var dbName = configuration.GetSection("MongoDbConfig:DBName").Value;

            // My Cluster plan is M0 which doesnt support sharding. To enable it need to switch to M30

            //var client = new MongoClient(connectionString);
            //var adminDatabase = client.GetDatabase("admin");
            //var enableShardingCommand = new BsonDocument { { "enableSharding", dbName } };

            //adminDatabase.RunCommand<BsonDocument>(enableShardingCommand);

            //var shardCollectionCommand = new BsonDocument
            //{
            //    { "shardCollection", $"{dbName}.Venues" },
            //    { "key", new BsonDocument { { "shardKeyField", 1 } } }
            //};

            //adminDatabase.RunCommand<BsonDocument>(shardCollectionCommand);

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(dbName);
            var collection = db.GetCollection<VenueEntity>("Venues");

            collection.Indexes.CreateOne(new CreateIndexModel<VenueEntity>(Builders<VenueEntity>.IndexKeys.Ascending(x => x.VenueCategoryId)));

            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                return new MongoClient(settings);
            });
            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(dbName);
            });

            services.AddIdentity<UserEntity, RoleEntity>(o =>
                {
                    o.User.RequireUniqueEmail = true;
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 3;
                })
                .AddDefaultTokenProviders();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("RedisCacheSettings:ConnectionString").Value;
            });

            services.AddScoped<IUserStore<UserEntity>, UserStore>();
            services.AddScoped<IRoleStore<RoleEntity>, RoleStore>();

            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<IVenueCategoryRepository, VenueCategoryRepository>();
            services.AddScoped<ISpacecraftService, SpacecraftService>();

            services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
            services.Configure<UsersConfiguration>(configuration.GetSection("Users"));
            services.Configure<SpacecraftApiConfiguration>(configuration.GetSection("SpacecraftApi"));
            services.Configure<VenuesApiConfiguration>(configuration.GetSection("VenuesApi"));

            services.AddHttpClient();

            services.AddTransient<ExceptionMiddleware>();
            services.AddTransient<AuthorizationMiddleware>();

            //services.AddScoped<IJwtService, JwtService>();

            //services.Configure<AuthorizationConfiguration>(configuration.GetSection("Authorization"));
            //services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
            //services.Configure<UsersConfiguration>(configuration.GetSection("Users"));

            return services;
        }
    }
}
