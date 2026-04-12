namespace WasmCore1.Models.States;

public class AuthState
{
    public bool   IsAuthenticated { get; set; }
    public string Uid             { get; set; } = "";
    public string Email           { get; set; } = "";
}
