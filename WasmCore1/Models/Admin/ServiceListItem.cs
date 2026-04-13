using WasmCore1.Models.__Base__;

namespace WasmCore1.Models.Admin;

public sealed class ServiceListItem
{
    public string Category { get; set; } = "";
    public BaseSvcStructure Service { get; set; } = new();
}