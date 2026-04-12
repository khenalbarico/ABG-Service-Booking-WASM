using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace WasmTools1.Api;

public sealed class ApiClient(HttpClient _httpClient, IOptions<ApiClientOpts> _opts) : IApiClient
{
    public async Task<T> GetAsync<T>(
           string            className,
           string            methodName,
           CancellationToken ct      = default)
    {
        var req = new RelayReq
        {
            ClassName  = className,
            MethodName = methodName,
            Payload    = null
        };

        using var resp = await _httpClient.PostAsJsonAsync(_opts.Value.RelayPath, req, ct);

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}");
        }

        var result = await resp.Content.ReadFromJsonAsync<T>(cancellationToken: ct);

        return result ?? throw new InvalidOperationException("The API returned no content.");
    }

    public async Task<T> SubmitAsync<T>(
           string            className,
           string            methodName,
           object?           payload = null,
           CancellationToken ct      = default)
    {
        var req = new RelayReq
        {
            ClassName  = className,
            MethodName = methodName,
            Payload    = payload
        };
        using var resp = await _httpClient.PostAsJsonAsync(_opts.Value.RelayPath, req, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}");
        }
        var result = await resp.Content.ReadFromJsonAsync<T>(cancellationToken: ct);

        return result ?? throw new InvalidOperationException("The API returned no content.");
    }

    public async Task PostAsync(
           string            className,
           string            methodName,
           object?           payload = null,
           CancellationToken ct      = default)
    {
        var req = new RelayReq
        {
            ClassName  = className,
            MethodName = methodName,
            Payload    = payload
        };

        using var resp = await _httpClient.PostAsJsonAsync(_opts.Value.RelayPath, req, ct);

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}");
        }
    }
}