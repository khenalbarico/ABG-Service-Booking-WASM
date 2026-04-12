using WasmCore1.Models.Client;
using WasmTools1.Api;

namespace WasmCore1.Services;

public class AppEmailer (IApiClient _apiClient)
{
    public async Task SendEmailAsync(ClientRequest req, CancellationToken ct = default)
    => await _apiClient.PostAsync("IAppEmailer", "SendEmailAsync", req, ct);
}
