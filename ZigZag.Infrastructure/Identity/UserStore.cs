using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using ZigZag.Domain.Entities.Identity;

namespace ZigZag.Infrastructure.Identity
{
    public class UserStore : IUserStore<UserEntity>, IUserPasswordStore<UserEntity>, IUserEmailStore<UserEntity>, IUserRoleStore<UserEntity>
    {
        private readonly IMongoCollection<UserEntity> _usersCollection;
        private readonly IMongoCollection<RoleEntity> _rolesCollection;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        public UserStore(
            IMongoDatabase database,
            IPasswordHasher<UserEntity> passwordHasher)
        {
            _usersCollection = database.GetCollection<UserEntity>("Users");
            _rolesCollection = database.GetCollection<RoleEntity>("Roles");
            _passwordHasher = passwordHasher;
        }

        public async Task<UserEntity> FindByIdAsync(ObjectId userId, CancellationToken cancellationToken)
        {
            return await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task AddToRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            var role = await _rolesCollection.Find(r => r.NormalizedName == roleName.ToUpper()).FirstOrDefaultAsync(cancellationToken);
            if (role == null)
            {
                throw new InvalidOperationException($"Role {roleName} does not exist.");
            }

            if (!user.Roles.Contains(role.NormalizedName))
            {
                user.Roles.Add(role.NormalizedName);
                await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
            }
        }

        public async Task RemoveFromRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            if (user.Roles.Contains(roleName.ToUpper()))
            {
                user.Roles.Remove(roleName.ToUpper());
                await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
            }
        }

        public async Task<IList<string>> GetRolesAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return user.Roles;
        }

        public async Task<bool> IsInRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            return user.Roles.Contains(roleName.ToUpper());
        }

        public async Task<IList<UserEntity>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var users = await _usersCollection.Find(u => u.Roles.Contains(roleName.ToUpper())).ToListAsync(cancellationToken);
            return users;
        }

        public async Task<UserEntity> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _usersCollection.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UserEntity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _usersCollection.Find(u => u.Id == ObjectId.Parse(userId)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UserEntity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _usersCollection.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public Task<string> GetEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public void Dispose() { }

        public Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken) => Task.FromResult(user.Id.ToString());
        public Task<string> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);
        public Task SetUserNameAsync(UserEntity user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(UserEntity user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(UserEntity user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);
        public Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash != null);

        public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
        {
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return passwordVerificationResult == PasswordVerificationResult.Success;
        }

        public Task SetEmailAsync(UserEntity user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(UserEntity user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }
    }
}