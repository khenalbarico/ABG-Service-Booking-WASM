using WasmCore1.Models.__Base__;

namespace WasmCore1.Algorithms;

public static class ServiceSectionKeyAlgorithms
{
    public static string BuildCardKey(string title, BaseSvcStructure svc)
        => $"{title}|{svc.Uid}|{svc.Details}|{svc.Cost}";
}