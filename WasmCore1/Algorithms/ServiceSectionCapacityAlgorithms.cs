using WasmCore1.ApiModels;
using WasmCore1.Models.__Base__;
using WasmCore1.Models.Schedules;

namespace WasmCore1.Algorithms;

public static class ServiceSectionCapacityAlgorithms
{
    public static bool IsTimeSlotFull(
           BaseSvcStructure   svc,
           string             title,
           DateTime           date,
           string             timeSlot,
           List<ApptSchedRec> appointmentSchedules,
           ScheduleCfg        scheduleCfg)
    {
        if (!TryGetCapacityForSlot(svc, title, timeSlot, scheduleCfg, out var capacity))
            return false;

        var slotDateTime = ServiceSectionTimeAlgorithms.CombineDateAndTime(date, timeSlot);

        var bookedCount = appointmentSchedules
            .Where(x => x.ServiceDate == slotDateTime)
            .Where(x => MatchesCapacityCategory(svc, title, x))
            .Select(x => x.ClientBookingId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();

        return bookedCount >= capacity;
    }

    public static string GetFullSlotLabel(BaseSvcStructure svc, string title, string timeSlot)
    {
        if (IsFootspaOrPedicureService(title))
            return $"{timeSlot} - Booked by Footspa or Pedicure";

        return $"{timeSlot} - Full";
    }

    public static string GetFullSlotMessage(BaseSvcStructure svc, string title, string timeSlot)
    {
        if (IsFootspaOrPedicureService(title))
            return "This time is not available because it is already booked by a Footspa or Pedicure appointment.";

        return "This time slot is already full for selected bookings.";
    }

    public static bool TryGetCapacityForSlot(
           BaseSvcStructure svc,
           string           title,
           string           timeSlot,
           ScheduleCfg      scheduleCfg,
           out int          capacity)
    {
        capacity = 0;

        var map = GetCapacityMap(title, scheduleCfg);

        if (map.TryGetValue(timeSlot, out capacity))
            return true;

        var normalizedSelected = ServiceSectionTimeAlgorithms.NormalizeTimeRangeLabel(timeSlot);

        foreach (var kvp in map)
        {
            if (string.Equals(ServiceSectionTimeAlgorithms.NormalizeTimeRangeLabel(kvp.Key), normalizedSelected, StringComparison.OrdinalIgnoreCase))
            {
                capacity = kvp.Value;
                return true;
            }
        }

        return false;
    }

    public static Dictionary<string, int> GetCapacityMap(string title, ScheduleCfg scheduleCfg)
    {
        if (IsNailService(title))
            return scheduleCfg.NailsAccommodationCapacities;

        if (IsFootspaOrPedicureService(title))
            return scheduleCfg.OtherServicesAccommodationCapacities.ToDictionary(x => x.Key, _ => 1);

        return scheduleCfg.OtherServicesAccommodationCapacities;
    }

    public static bool MatchesCapacityCategory(BaseSvcStructure svc, string title, ApptSchedRec record)
    {
        if (IsNailService(title))
            return record.Services.Any(IsNailServiceRecord);

        if (IsFootspaOrPedicureService(title))
            return record.Services.Any(IsFootspaOrPedicureServiceRecord);

        return record.Services.Any(IsRegularOtherServiceRecord);
    }

    public static bool IsNailService(string title)
        => title.Equals("Nails", StringComparison.OrdinalIgnoreCase);

    public static bool IsFootspaService(string title)
        => title.Equals("Footspa", StringComparison.OrdinalIgnoreCase);

    public static bool IsPedicureService(string title)
        => title.Equals("Pedicure", StringComparison.OrdinalIgnoreCase);

    public static bool IsFootspaOrPedicureService(string title)
        => IsFootspaService(title) || IsPedicureService(title);

    public static bool IsNailServiceRecord(ApptSchedService service)
        => service.ServiceName.Equals("Nails", StringComparison.OrdinalIgnoreCase);

    public static bool IsFootspaServiceRecord(ApptSchedService service)
        => service.ServiceName.Equals("Footspa", StringComparison.OrdinalIgnoreCase);

    public static bool IsPedicureServiceRecord(ApptSchedService service)
        => service.ServiceName.Equals("Pedicure", StringComparison.OrdinalIgnoreCase);

    public static bool IsFootspaOrPedicureServiceRecord(ApptSchedService service)
        => IsFootspaServiceRecord(service) || IsPedicureServiceRecord(service);

    public static bool IsRegularOtherServiceRecord(ApptSchedService service)
        => !IsNailServiceRecord(service) && !IsFootspaOrPedicureServiceRecord(service);
}