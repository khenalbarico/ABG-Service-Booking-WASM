
namespace WasmTools1.Api;

public class ApiClient : IApiClient
{
    public Task<T> GetAsync<T>(string className, string methodName)
    {
        throw new NotImplementedException();
    }

    public Task PostAsync<T>(string className, string methodName, T data)
    {
        throw new NotImplementedException();
    }

    public Task<T> Submit<T>(string className, string methodName, T data)
    {
        throw new NotImplementedException();
    }
}
