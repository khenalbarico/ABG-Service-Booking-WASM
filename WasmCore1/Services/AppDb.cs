using WasmCore1.ApiModels;
using WasmCore1.Models.__Base__;
using WasmCore1.Models.Client;
using WasmCore1.Models.Schedules;
using WasmCore1.Models.Service;
using WasmTools1.Api;

namespace WasmCore1.Services;

public class AppDb (IApiClient _apiClient)
{
    public async Task<ServiceCollectionResp> GetServicesAsync(CancellationToken ct = default)
    => await _apiClient.GetAsync<ServiceCollectionResp>("IAppDbOperator", "GetServicesAsync", ct);
    
    public async Task<List<ApptSchedRec>> GetAppointmentSchedulesAsync(CancellationToken ct = default)
    => await _apiClient.GetAsync<List<ApptSchedRec>>("IAppDbOperator", "GetAppointmentSchedulesAsync", ct);

    public async Task PostClientRequestAsync(ClientRequest req, CancellationToken ct = default)
    => await _apiClient.PostAsync("IAppDbOperator", "PostClientRequestAsync", new { req }, ct);

    public async Task<List<ClientRequest>> GetClientRequestsAsync(CancellationToken ct = default)
    => await _apiClient.GetAsync<List<ClientRequest>>("IAppDbOperator", "GetClientRequestsAsync", ct);

    public async Task SaveServiceAsync(string category, BaseSvcStructure service, CancellationToken ct = default)
    => await _apiClient.PostAsync("IAppDbOperator", "SaveServiceAsync", new { category, service }, ct);

    public async Task DeleteServiceAsync(string category, string serviceUid, CancellationToken ct = default)
    => await _apiClient.PostAsync("IAppDbOperator", "DeleteServiceAsync", new { category, serviceUid }, ct);

    public async Task<ScheduleCfg> GetScheduleCfgAsync(CancellationToken ct = default)
    => await _apiClient.GetAsync<ScheduleCfg>("IAppDbOperator", "GetScheduleCfgAsync", ct);

    public async Task PostScheduleCfgAsync(ScheduleCfg cfg, CancellationToken ct = default)
    => await _apiClient.PostAsync("IAppDbOperator", "PostScheduleCfgAsync", new { cfg }, ct);
}
