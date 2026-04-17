using WasmCore1.ApiModels;
using WasmCore1.Models.Client;
using WasmTools1.Api;

namespace WasmCore1.Services;

public class AppPayment (IApiClient _apiClient, AppGlobalError _globalError)
{
    public async Task<PaymongoQrphChargeResult> CreateQrphChargeAsync(ClientRequest req, CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.SubmitAsync<PaymongoQrphChargeResult>("IToolPaymentApi", "CreateQrphChargeAsync", new { req }, ct);

            return resp;
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task<string> ProcessClientPaymentAsync(
           string            paymentIntentId,
           ClientRequest     req,
           CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.SubmitAsync<string>("IAppPaymentApi", "ProcessClientPaymentAsync", new { paymentIntentId, req }, ct);

            return resp;
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }
}
