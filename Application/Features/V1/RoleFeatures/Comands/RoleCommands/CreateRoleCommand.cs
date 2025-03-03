﻿using MediatR;
using Models.ResponseModels;
using Models.Role;

namespace Application.Features.V1.RoleFeatures.Comands.RoleCommands
{
    public record CreateRoleCommand(CreateRoleModel CreateRoleModel) : IRequest<Response<CreateRoleModel>>;

}
