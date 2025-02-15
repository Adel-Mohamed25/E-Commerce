
using Services.IServices;

namespace Services.UnitOfServices
{
    public class UnitOfService : IUnitOfService
    {
        public IAuthServices AuthServices { get; private set; }

        public IEmailServices EmailServices { get; private set; }

        public UnitOfService(
            IAuthServices authServices,
            IEmailServices emailServices)
        {
            AuthServices = authServices;
            EmailServices = emailServices;
        }
    }
}
