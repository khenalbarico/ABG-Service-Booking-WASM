using Microsoft.Extensions.DependencyInjection;
using WasmCore1.ApiModels;
using WasmCore1.Models.States;
using WasmCore1.Services;
using WasmTools1.Api;

namespace WasmCore1;

public static class SvcRegistry
{
    public static void RegisterSvc(this IServiceCollection svc)
    {
        svc.AddScoped<IApiClient, ApiClient>();
        svc.AddScoped<AppDb>();
        svc.AddScoped<AppEmailer>();
        svc.AddScoped<AppPayment>();
        svc.AddScoped<AppAuth>();
        svc.AddScoped<AuthState>();
    }
}
