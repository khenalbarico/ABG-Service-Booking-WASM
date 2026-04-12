using Firebase.Auth;
using WasmTools1.Api;

namespace WasmCore1.Services;

public class AppAuth (IApiClient _apiClient)
{
    public async Task<UserCredential> LoginAsync(string email, string password, CancellationToken ct = default)
    => await _apiClient.SubmitAsync<UserCredential>("IAppAuthentication", "LoginAsync", new { email, password }, ct);
}
