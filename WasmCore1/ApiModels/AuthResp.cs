namespace WasmCore1.ApiModels;

public class AuthResp
{
    public bool    IsAuthenticated { get; set; }
    public string  Uid             { get; set; } = "";
    public string  Email           { get; set; } = "";
}
