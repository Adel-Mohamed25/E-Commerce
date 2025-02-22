using Infrastructure.UnitOfWorks;

namespace Infrastructure.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedUserAsync(IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
        }
    }
}
