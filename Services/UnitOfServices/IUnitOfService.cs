using Services.Abstractions;
using Services.Abstractios;

namespace Services.UnitOfServices
{
    public interface IUnitOfService
    {
        IAuthServices AuthServices { get; }
        IEmailServices EmailServices { get; }
        IFileServices FileServices { get; }
    }
}
