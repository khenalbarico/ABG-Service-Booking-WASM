using System.Globalization;
using ToolsLib1.ClientModels.Schedules;
using WasmCore1.ApiModels;
using WasmCore1.Models.Client;
using WasmCore1.Models.Schedules;

namespace WasmCore1.Algorithms;

public static class SlotChecker
{
    public static SlotCheckRes ValidateClientReq(
        this ClientRequest      req,
             List<ApptSchedRec> schedules,
             ScheduleCfg        cfg)
    {
        var grouped = (req.ClientServices ?? [])
            .GroupBy(x => x.ServiceDate);

        foreach (var g in grouped)
        {
            var hasNail = g.Any(x => x.IsNailService());

            var timeKey = g.Key.ToString("h:mm tt", CultureInfo.InvariantCulture);

            var map = hasNail
                ? cfg.NailsAccommodationCapacities
                : cfg.OtherServicesAccommodationCapacities;

            if (map is null || !map.TryGetValue(timeKey, out var capacity))
                continue;

            var count = schedules
                .Where(x => x.ServiceDate == g.Key)
                .Where(x => hasNail ? x.IsNailBooking() : !x.IsNailBooking())
                .Select(x => x.ClientBookingId)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            if (count >= capacity)
            {
                return new SlotCheckRes
                {
                    IsAvailable = false,
                    Message = $"Sorry, the slot on {g.Key:MMMM dd, yyyy} at {g.Key:hh:mm tt} is already full. Please choose another schedule."
                };
            }
        }

        return new SlotCheckRes { IsAvailable = true };
    }
}