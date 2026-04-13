namespace WasmCore1.Algorithms;

public static class CheckoutPaymentAlgorithms
{
    public static bool IsPaymentSuccessful(string? status)
        => string.Equals(status, "succeeded", StringComparison.OrdinalIgnoreCase);
}