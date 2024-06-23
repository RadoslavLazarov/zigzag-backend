using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using ZigZag.Domain.Common.Eums;
using ZigZag.Domain.Configurations;
using ZigZag.Domain.Configurations.UsersConfiguration;
using ZigZag.Domain.Entities.Identity;
using ZigZag.Domain.Entities.Venue;
using ZigZag.Domain.Models.Venue;

namespace ZigZag.Infrastructure
{
    public static class DbInitializer
    {
        public static async void Seed(IServiceProvider services)
        {
            var scope = services.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase> () ?? throw new ArgumentNullException(nameof(IMongoDatabase));
            var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>() ?? throw new ArgumentNullException(nameof(HttpClient));
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>() ?? throw new ArgumentNullException(nameof(UserEntity));
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>() ?? throw new ArgumentNullException(nameof(RoleEntity));
            var venuesConfiguration = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<VenuesApiConfiguration>>().CurrentValue ?? throw new ArgumentNullException(nameof(VenuesApiConfiguration));
            var usersConfiguration = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<UsersConfiguration>>().CurrentValue ?? throw new ArgumentNullException(nameof(UsersConfiguration));

            var venuesCollection = database.GetCollection<VenueEntity>("Venues");
            var venueCategoriesCollection = database.GetCollection<VenueCategoryEntity>("VenueCategories");

            #region Venues   

            var venueExsists = (await venuesCollection.CountDocumentsAsync(FilterDefinition<VenueEntity>.Empty)) > 0;
            if (!venueExsists)
            {
                var response = await httpClient.GetAsync(venuesConfiguration.BaseUrl);
                if (response.IsSuccessStatusCode)
                {      
                    var result = await response.Content.ReadAsStringAsync();
                    var responseModel = JsonSerializer.Deserialize<VenuesApiModel>(result);

                    var venueCategoryExsists = (await venueCategoriesCollection.CountDocumentsAsync(FilterDefinition<VenueCategoryEntity>.Empty)) > 0;
                    if (!venueCategoryExsists)
                    {
                        var venueCategories = responseModel.Venues.Select(x => x.CategoryName).Distinct().ToList();

                        var venueCategoryEntities = venueCategories.Select(category => new VenueCategoryEntity
                        {
                            Id = new ObjectId(),
                            Name = category
                        });

                        await venueCategoriesCollection.InsertManyAsync(venueCategoryEntities);
                    }

                    var venueCategoriesEnities = (await venueCategoriesCollection.FindAsync(FilterDefinition<VenueCategoryEntity>.Empty)).ToList();

                    var venueEntities = responseModel.Venues.Select(venue => new VenueEntity
                    {
                        Name = venue.Name,
                        VenueCategoryId = (venueCategoriesEnities.First(category => category.Name == venue.CategoryName)).Id
                    });

                    await venuesCollection.InsertManyAsync(venueEntities);
                }
            }

            #endregion

            #region Roles

            foreach (string name in Enum.GetNames(typeof(RoleType)))
            {
                var role = await roleManager.FindByNameAsync(name);
                if (role == null)
                {
                    await roleManager.CreateAsync(
                        new RoleEntity()
                        {
                            Name = name,
                        });
                }
            }

            #endregion

            #region System Users

            var admin = await userManager.FindByEmailAsync(usersConfiguration.Admin.Email);
            if (admin == null)
            {
                await CreateUser(usersConfiguration.Admin.Username, usersConfiguration.Admin.Email, usersConfiguration.Admin.Password, RoleType.Admin);
            }

            var client = await userManager.FindByEmailAsync(usersConfiguration.Client.Email);
            if (client == null)
            {
                await CreateUser(usersConfiguration.Client.Username, usersConfiguration.Client.Email, usersConfiguration.Client.Password, RoleType.Client);
            }

            async Task<UserEntity> CreateUser(string userName, string email, string password, RoleType role)
            {
                var userEntity = new UserEntity()
                {
                    UserName = userName,
                    Email = email,
                    IsSystem = true
                };

                var userResult = await userManager.CreateAsync(userEntity, password);
                if (!userResult.Succeeded)
                {
                    throw new Exception($"User with email {email} creation failed");
                }

                var newUser = await userManager.FindByEmailAsync(email);
                await userManager.AddToRoleAsync(newUser, role.ToString());

                return newUser;
            }

            #endregion
        }  
    }
}
