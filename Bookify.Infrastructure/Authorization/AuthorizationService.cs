using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class AuthorizationService
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorizationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRolesResponse> GetRolesForUserAsync(string IdentityId)
        {
            var roles = await _dbContext.Set<User>()
                .Where(user => user.IdentityId == IdentityId)
                .Select(user => new UserRolesResponse
                {
                    Id = user.Id,
                    Roles = user.Roles.ToList()
                })
                .FirstAsync();

            return roles;
        }

        public async Task<HashSet<string>> GetPermissionForUserAsync(string identityId)
        {
            var permissions = await _dbContext.Set<User>()
                .Where(user => user.IdentityId == identityId)
                .SelectMany(user => user.Roles.Select(role => role.Permissions))
                .FirstAsync();

            var permissionsSet = permissions.Select(permission => permission.Name).ToHashSet();

            return permissionsSet;
        }
    }
}
