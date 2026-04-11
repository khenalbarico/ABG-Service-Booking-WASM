using static WasmCore1.Models.Constants;

namespace WasmCore1.Models.Client;

public sealed class ClientService
{
    public string              ServiceUid     { get; set; } = "";
    public string              ServiceName    { get; set; } = "";
    public string              ServiceDesign  { get; set; } = "";
    public string              ServiceDetails { get; set; } = "";
    public decimal             ServiceCost    { get; set; }
    public ServiceBranch       Branch         { get; set; } = ServiceBranch.Anabu;
    public DateTime            ServiceDate    { get; set; }
    public ClientServiceStatus Status         { get; set; } = ClientServiceStatus.Pending;
}
