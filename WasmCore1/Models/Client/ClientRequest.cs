using static WasmCore1.Models.Constants;

namespace WasmCore1.Models.Client;

public sealed class ClientRequest
{
    public ClientInformation   ClientInformation { get; set; } = new();
    public List<ClientService> ClientServices    { get; set; } = [];
    public ClientStatus        Status            { get; set; } = ClientStatus.Pending;
}
