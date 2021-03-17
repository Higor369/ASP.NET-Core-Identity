﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace WebApplication.identity
{
    public class MyUserStore : IUserStore<MyUser>, IUserPasswordStore<MyUser>
    {
        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = OpenDbConection())
            {
                await connection.ExecuteAsync(
                    "insert into Users([Id], [UserName], [NormalizedUsername], [PasswordHash]) " +
                    "Values ( @id, @userName, @normalizedUserName , @passwordHash) ",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUsername,
                        passwordHash = user.PasswordHash
                    });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = OpenDbConection())
            {
                await connection.ExecuteAsync(
                    "delete from Users where Id = @id",
                    new
                    {
                        id = user.Id
                    });
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }

        private static DbConnection OpenDbConection()
        {
            var connection = new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=cursoIdentity;Data Source=DESKTOP-2MHR3Q1\ROOT");

            connection.Open();

            return connection;
        }

        public async Task<MyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using(var connection = OpenDbConection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "select * from Users where Id = @id",
                    new { id = userId });
            }
        }

        public async Task<MyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = OpenDbConection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "select * from Users where normalizedUserName = @normalizedUserName",
                    new { normalizedUserName = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUsername); 
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUsername = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = OpenDbConection())
            {
                await connection.ExecuteAsync(
                    "update User set " +
                    "[Id] = @id, " +
                    "[UserName] = @userName, " +
                    "[NormalizedUsername] = @normalizedUserName, " +
                    "[PasswordHash] = @passwordHash, " +
                    "where [Id] = @id",
                    new { 
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUsername,
                        passwordHash = user.PasswordHash
                    });
            }

            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(MyUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
