using BlazorApp1;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using WasmCore1;
using WasmTools1.Api;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<ApiClientOpts>(builder.Configuration.GetSection("ApiClient"));

builder.Services.AddScoped(sp =>
{
    var opts = sp.GetRequiredService<IOptions<ApiClientOpts>>().Value;
    return new HttpClient
    {
        BaseAddress = new Uri(opts.BaseUrl)
    };
});

builder.Services.RegisterSvc();

await builder.Build().RunAsync();
