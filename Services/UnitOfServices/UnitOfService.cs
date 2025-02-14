
using Services.IServices;

namespace Services.UnitOfServices
{
    public class UnitOfService : IUnitOfService
    {
        public IAuthenticationServices AuthenticationServices { get; private set; }

        public IEmailServices EmailServices { get; private set; }

        public UnitOfService(
            IAuthenticationServices authenticationServices,
            IEmailServices emailServices)
        {
            AuthenticationServices = authenticationServices;
            EmailServices = emailServices;
        }
    }
}
