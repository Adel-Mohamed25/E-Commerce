﻿using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Contracts.Contracts.IIdentityRepository
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public RoleManager<Role> RoleManager { get; }
    }
}
