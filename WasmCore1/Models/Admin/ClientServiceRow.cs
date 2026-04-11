using WasmCore1.Models.Client;

namespace WasmCore1.Models.Admin;

public class ClientServiceRow
{
    public required ClientRequest Request { get; init; }
    public required ClientService Service { get; init; }
}
