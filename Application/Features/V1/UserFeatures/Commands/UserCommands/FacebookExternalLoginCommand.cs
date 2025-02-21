﻿using MediatR;
using Models.Authentication;
using Models.ResponseModels;

namespace Application.Features.V1.UserFeatures.Commands.UserCommands
{
    public record FacebookExternalLoginCommand(ExternalAuthRequest ExternalAuth) : IRequest<Response<AuthModel>>;
}
