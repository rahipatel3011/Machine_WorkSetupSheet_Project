namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface ICacheService
    {
        Task<T> Get<T>(string key);
        bool Save<T>(string key, T value, DateTimeOffset expirytime);
        Task<bool> Delete(string key);
    }
}
