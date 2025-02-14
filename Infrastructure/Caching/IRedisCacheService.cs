namespace Infrastructure.Caching
{
    public interface IRedisCacheService
    {
        T? GetData<T>(string key);
        void SetData<T>(string key, T data);

        void RemoveData(string key);
    }
}
