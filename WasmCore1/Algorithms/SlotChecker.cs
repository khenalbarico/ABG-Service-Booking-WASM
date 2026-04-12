using System.Globalization;
using ToolsLib1.ClientModels.Schedules;
using WasmCore1.Models.Client;
using WasmCore1.Models.Schedules;

namespace WasmCore1.Algorithms;

public static class SlotChecker
{
    public static SlotCheckRes ValidateClientReq(
           this   ClientRequest req,
           List<ApptSchedRec>   schedules)
    {
        var groupedServices = (req.ClientServices ?? [])
            .GroupBy(x => x.ServiceDate);

        foreach (var group in groupedServices)
        {
            var requestedServices = group.ToList();

            if (!requestedServices.Any(x => x.IsNailService()))
                continue;

            var normalizedTime = group.Key.ToString("h:mm tt", CultureInfo.InvariantCulture);

            if (!Capacities.NailSlotCapacities.TryGetValue(normalizedTime, out var capacity))
                continue;

            var bookedCount = schedules
                .Where(x => x.ServiceDate == group.Key)
                .Where(x => x.IsNailBooking())
                .Select(x => x.ClientBookingId)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            if (bookedCount >= capacity)
            {
                return new SlotCheckRes
                {
                    IsAvailable = false,
                    Message = $"Sorry, the slot on {group.Key:MMMM dd, yyyy} at {group.Key:hh:mm tt} is already full. Please choose another schedule."
                };
            }
        }

        return new SlotCheckRes
        {
            IsAvailable = true,
            Message = ""
        };
    }
}