using WasmCore1.Models.Client;
using WasmTools1.Api;

namespace WasmCore1.Services;

public class AppEmailer (IApiClient _apiClient, AppGlobalError _globalError)
{
    public async Task SendEmailAsync(ClientRequest req, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IAppEmailer", "SendEmailAsync", req, ct);
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }
}
