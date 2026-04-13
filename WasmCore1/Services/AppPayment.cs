using WasmCore1.ApiModels;
using WasmCore1.Models.Client;
using WasmTools1.Api;

namespace WasmCore1.Services;

public class AppPayment (IApiClient _apiClient)
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
            throw new Exception(ex.Message);
        }
    }
    public async Task<string> GetPaymentIntentStatusAsync(string paymentIntentId, CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.SubmitAsync<string>("IToolPaymentApi", "GetPaymentIntentStatusAsync", new { paymentIntentId }, ct);

            return resp;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
