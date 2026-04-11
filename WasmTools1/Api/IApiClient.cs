namespace WasmTools1.Api;

public interface IApiClient
{
    Task<T> GetAsync<T>(string className, string methodName);
    Task<T> Submit<T>(string className, string methodName, T data);
    Task PostAsync<T>(string className, string methodName, T data);
}
