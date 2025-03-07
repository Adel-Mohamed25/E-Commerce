
using Services.Abstractions;
using Services.Abstractios;

namespace Services.UnitOfServices
{
    public class UnitOfService : IUnitOfService
    {
        public IAuthServices AuthServices { get; private set; }

        public IEmailServices EmailServices { get; private set; }

        public IFileServices FileServices { get; private set; }

        public UnitOfService(
            IAuthServices authServices,
            IEmailServices emailServices,
            IFileServices fileServices)
        {
            AuthServices = authServices;
            EmailServices = emailServices;
            FileServices = fileServices;
        }
    }
}
