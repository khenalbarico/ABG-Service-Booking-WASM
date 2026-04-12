namespace WasmCore1.ApiModels;

public class ScheduleCfg
{
    public List<string>            StoreHours                           { get; set; } = [];

    public Dictionary<string, int> NailsAccommodationCapacities         { get; set; } = [];

    public Dictionary<string, int> OtherServicesAccommodationCapacities { get; set; } = [];
}
