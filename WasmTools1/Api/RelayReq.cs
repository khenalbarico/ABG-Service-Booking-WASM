namespace WasmTools1.Api;

public sealed class RelayReq
{
    public string  ClassName  { get; set; } = string.Empty;
    public string  MethodName { get; set; } = string.Empty;
    public object? Payload    { get; set; }
}