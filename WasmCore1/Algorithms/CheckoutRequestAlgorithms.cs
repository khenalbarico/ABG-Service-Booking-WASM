using WasmCore1.Models.Client;

namespace WasmCore1.Algorithms;

public static class CheckoutRequestAlgorithms
{
    public static void PrepareClientInformation(ClientRequest request)
    {
        var now = DateTime.Now;

        request.ClientInformation.BookingDate = now;
        request.ClientInformation.ClientBookingId = $"{now:MMddyy}-{Random.Shared.Next(10000000, 99999999)}";
    }
}