namespace WasmTools1.Api;

public interface IApiClient
{
    Task<T> GetAsync<T>(
           string            className,
           string            methodName,
           CancellationToken ct      = default);

    Task<T> SubmitAsync<T>(
          string            className,
          string            methodName,
          object?           payload = null,
          CancellationToken ct      = default);

    Task PostAsync(
           string            className,
           string            methodName,
           object?           payload = null,
           CancellationToken ct      = default);
}
