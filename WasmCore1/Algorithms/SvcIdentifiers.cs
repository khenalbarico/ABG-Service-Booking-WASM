using WasmCore1.Models.Client;
using WasmCore1.Models.Schedules;

namespace WasmCore1.Algorithms;

public static class SvcIdentifiers
{
    public static bool IsNailBooking(this ApptSchedRec record)
    {
        if (record.Services is null || record.Services.Count == 0)
            return true;

        return record.Services.Any(x =>
            (!string.IsNullOrWhiteSpace(x.ServiceName) && x.ServiceName.Contains("nail", StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrWhiteSpace(x.ServiceDetails) && x.ServiceDetails.Contains("nail", StringComparison.OrdinalIgnoreCase)));
    }

    public static bool IsNailService(this ClientService service)
    {
        return (!string.IsNullOrWhiteSpace(service.ServiceName) && service.ServiceName.Contains("nail", StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(service.ServiceDetails) && service.ServiceDetails.Contains("nail", StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrWhiteSpace(service.ServiceUid) && service.ServiceUid.Contains("nail", StringComparison.OrdinalIgnoreCase));
    }
}
