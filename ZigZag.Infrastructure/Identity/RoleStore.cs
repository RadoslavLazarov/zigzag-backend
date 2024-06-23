using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using ZigZag.Domain.Entities.Identity;

namespace ZigZag.Infrastructure.Identity
{
    public class RoleStore : IRoleStore<RoleEntity>
    {
        private readonly IMongoCollection<RoleEntity> _rolesCollection;

        public RoleStore(IMongoDatabase database)
        {
            _rolesCollection = database.GetCollection<RoleEntity>("Roles");
        }

        public async Task<IdentityResult> CreateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            await _rolesCollection.InsertOneAsync(role, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            await _rolesCollection.DeleteOneAsync(r => r.Id == role.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<RoleEntity> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _rolesCollection.Find(r => r.Id == ObjectId.Parse(roleId)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<RoleEntity> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _rolesCollection.Find(r => r.NormalizedName == normalizedRoleName).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            await _rolesCollection.ReplaceOneAsync(r => r.Id == role.Id, role, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose() { }

        public Task<string> GetRoleIdAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(RoleEntity role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(RoleEntity role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }
    }
}