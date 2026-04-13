using WasmCore1.ApiModels;
using WasmTools1.Api;

namespace WasmCore1.Services;

public class AppAuth (IApiClient _apiClient, AppGlobalError _globalError)
{
    public async Task<AuthResp> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.SubmitAsync<AuthResp>("IAppAuthentication", "LoginAsync", new { email, password }, ct);

            return resp;
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }
}
