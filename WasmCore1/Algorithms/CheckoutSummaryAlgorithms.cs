using WasmCore1.Models.Client;
using static WasmCore1.Models.Constants;

namespace WasmCore1.Algorithms;

public static class CheckoutSummaryAlgorithms
{
    public static readonly Dictionary<ServiceBranch, string> BranchNames = new()
    {
        { ServiceBranch.Anabu,  "Anabu Doyets Imus Cavite" },
        { ServiceBranch.Manila, "The Manila Residence Tower II TAFT Manila" }
    };

    public static decimal GetTotalAmount(ClientRequest request)
        => request.ClientServices.Sum(x => x.ServiceCost);

    public static string GetBranchDisplayName(ServiceBranch branch)
    {
        if (BranchNames.TryGetValue(branch, out var branchName))
            return branchName;

        return branch.ToString();
    }
}