﻿using Infrastructure.Settings;
using Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Options;
using Models.Email;
using Services.IServices;

namespace Services.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly IUnitOfWork _unitOfWork;

        public EmailServices(IOptions<EmailSettings> emailSettings, IUnitOfWork unitOfWork)
        {
            _emailSettings = emailSettings;
            _unitOfWork = unitOfWork;
        }

        public Task<EmailModel> SendEmailAsync(SendEmailModel emailModel)
        {
            throw new NotImplementedException();
        }

        public Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailRequestModel emailRequestModel)
        {
            throw new NotImplementedException();
        }

    }
}
