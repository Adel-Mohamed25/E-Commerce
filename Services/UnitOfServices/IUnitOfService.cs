using Services.IServices;

namespace Services.UnitOfServices
{
    public interface IUnitOfService
    {
        IAuthenticationServices AuthenticationServices { get; }
        IEmailServices EmailServices { get; }
    }
}
