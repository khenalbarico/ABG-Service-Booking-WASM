using WasmCore1.ApiModels;
using WasmCore1.Models.__Base__;
using WasmCore1.Models.Client;
using WasmCore1.Models.Schedules;
using WasmCore1.Models.Service;
using WasmTools1.Api;
using static WasmCore1.Models.Constants;

namespace WasmCore1.Services;

public class AppDb (IApiClient _apiClient, AppGlobalError _globalError)
{
    public async Task<ServiceCollectionResp> GetServicesAsync(CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.GetAsync<ServiceCollectionResp>("IAppDbOperator", "GetServicesAsync", ct);

            return resp;
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }
    
    public async Task<List<ApptSchedRec>> GetAppointmentSchedulesAsync(CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.GetAsync<List<ApptSchedRec>>("IAppDbOperator", "GetAppointmentSchedulesAsync", ct);

            return resp;
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task PostClientRequestAsync(ClientRequest req, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IAppDbOperator", "PostClientRequestAsync", req, ct);
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task PatchClientStatusAsync(string bookingId, ClientStatus status, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IAppDbOperator", "PatchClientStatusAsync", new { bookingId, status }, ct);
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task PostClientApptSchedAsync(ClientRequest req, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IAppDbOperator", "PostClientApptSchedAsync", req, ct);
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task<List<ClientRequest>> GetClientRequestsAsync(CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.GetAsync<List<ClientRequest>>("IAppDbOperator", "GetClientRequestsAsync", ct);

            return resp;
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task SaveServiceAsync(string category, BaseSvcStructure service, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IAppDbOperator", "SaveServiceAsync", new { category, service }, ct);
        }
        catch(Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task DeleteServiceAsync(string category, string serviceUid, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IAppDbOperator", "DeleteServiceAsync", new { category, serviceUid }, ct);
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task<ScheduleCfg> GetScheduleCfgAsync(CancellationToken ct = default)
    {
        try
        {
            var resp = await _apiClient.GetAsync<ScheduleCfg>("IAppDbOperator", "GetScheduleCfgAsync", ct);

            return resp;
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task PostScheduleCfgAsync(ScheduleCfg cfg, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IAppDbOperator", "PostScheduleCfgAsync", cfg, ct);
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }

    public async Task ValidateAvailabilityAsync(ClientRequest req, CancellationToken ct = default)
    {
        try
        {
            await _apiClient.PostAsync("IBookingCapacity", "ValidateAvailabilityAsync", req, ct);
        }
        catch (Exception ex)
        {
            await _globalError.ShowAsync(ex);
            throw;
        }
    }
}
