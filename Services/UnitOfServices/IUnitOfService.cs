using Services.Abstractions;

namespace Services.UnitOfServices
{
    public interface IUnitOfService
    {
        IAuthServices AuthServices { get; }
        IEmailServices EmailServices { get; }
    }
}
