using HotChocolate.Types.MongoDb;
using MongoDB.Bson;
using ZigZag.Application;
using ZigZag.Domain.Entities.Venue;
using ZigZag.Infrastructure;
using ZigZag.Infrastructure.Middlewares;
using ZigZag.WebAPI.Mutations;
using ZigZag.WebAPI.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);


builder.Services
    .AddCors()
    .AddMemoryCache()
    .AddGraphQLServer()
    .AddAuthorization()
    .InitializeOnStartup()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<SpacecraftQueries>()
    .AddTypeExtension<VenueQueries>()
    .AddTypeExtension<AuthorizationMutations>()
    .AddType<VenueCategoryEntity>()
    .AddType<VenueEntity>()
    .BindRuntimeType<ObjectId, ObjectIdType>();

var app = builder.Build();

DbInitializer.Seed(app.Services);

// Global CORS policy
app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.ConfigureMiddlewares();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();
