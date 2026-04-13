using Microsoft.AspNetCore.Components;
using WasmCore1.Algorithms;
using WasmCore1.Models.Client;
using static WasmCore1.Models.Constants;

namespace BlazorApp1.Shared;

public partial class ServiceCheckout : IDisposable
{
    [Parameter] public ClientRequest         Request            { get; set; } = new();
    [Parameter] public EventCallback<string> OnRemove           { get; set; }
    [Parameter] public EventCallback         OnClose            { get; set; }
    [Parameter] public EventCallback         OnCompleted        { get; set; }
    [Parameter] public EventCallback         OnSchedulesChanged { get; set; }

    bool showForm;
    bool showSuccess;
    bool showQr;
    bool showNailsRules;
    bool showConsentForm;
    bool isLoading;

    bool nailsRulesAccepted;
    bool consentAccepted;

    string?                  qrImageUrl;
    string?                  paymentIntentId;
    CancellationTokenSource? pollCts;

    string? consumerError;

    private async Task Remove(string uid)
        => await OnRemove.InvokeAsync(uid);

    private void OpenForm()
        => showForm = true;

    private void CloseForm()
    {
        consumerError = null;
        showForm = false;
    }

    private decimal GetTotalAmount()
        => CheckoutSummaryAlgorithms.GetTotalAmount(Request);

    private string GetBranchDisplayName(ServiceBranch branch)
        => CheckoutSummaryAlgorithms.GetBranchDisplayName(branch);

    private async Task Submit()
    {
        consumerError = null;

        CheckoutRequestAlgorithms.PrepareClientInformation(Request);

        showForm = false;

        var nextStep = CheckoutPolicyAlgorithms.ResolveNextStep(
            Request,
            nailsRulesAccepted,
            consentAccepted);

        if (nextStep == CheckoutFlowStep.NailsRules)
        {
            showNailsRules = true;
            showConsentForm = false;
            return;
        }

        if (nextStep == CheckoutFlowStep.ConsentForm)
        {
            showNailsRules = false;
            showConsentForm = true;
            return;
        }

        await StartPaymentAsync();
    }

    private async Task HandleNailsRulesAccepted()
    {
        nailsRulesAccepted = true;
        showNailsRules = false;

        var nextStep = CheckoutPolicyAlgorithms.ResolveNextStep(
            Request,
            nailsRulesAccepted,
            consentAccepted);

        if (nextStep == CheckoutFlowStep.ConsentForm)
        {
            showConsentForm = true;
            return;
        }

        await StartPaymentAsync();
    }

    private void BackFromNailsRules()
    {
        showNailsRules = false;
        showForm = true;
    }

    private async Task HandleConsentAccepted()
    {
        consentAccepted = true;
        showConsentForm = false;
        await StartPaymentAsync();
    }

    private void BackFromConsentForm()
    {
        showConsentForm = false;

        if (CheckoutPolicyAlgorithms.RequiresNailsRules(Request) && nailsRulesAccepted)
        {
            showNailsRules = true;
            return;
        }

        showForm = true;
    }

    private async Task StartPaymentAsync()
    {
        isLoading = true;
        StateHasChanged();

                     await Db.ValidateAvailabilityAsync(Request);
        var result = await Payment.CreateQrphChargeAsync(Request);

        paymentIntentId = result.PaymentIntentId;
        qrImageUrl      = result.QrImageUrl;

        isLoading = false;
        showQr    = true;

        pollCts   = new CancellationTokenSource();
        _ = PollPaymentStatus(pollCts.Token);
    }

    private async Task PollPaymentStatus(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                var status = await Payment.GetPaymentIntentStatusAsync(paymentIntentId!);

                if (CheckoutPaymentAlgorithms.IsPaymentSuccessful(status))
                {
                    pollCts?.Cancel();

                    Request.Status = ClientStatus.Paid;
                    await Db.PostClientRequestAsync(Request);
                    await Emailer.SendEmailAsync(Request);
                    await OnSchedulesChanged.InvokeAsync();

                    ShowSuccessState();

                    await InvokeAsync(StateHasChanged);
                    break;
                }

                await Task.Delay(3000, ct);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    private void CancelPayment()
    {
        pollCts?.Cancel();
        showQr = false;
        paymentIntentId = null;
        qrImageUrl = null;
    }

    private async Task Close()
    {
        pollCts?.Cancel();
        await OnClose.InvokeAsync();
    }

    private async Task Finish()
    {
        pollCts?.Cancel();
        ResetCheckoutState();
        await OnCompleted.InvokeAsync();
    }

    private void ShowSuccessState()
    {
        showForm = false;
        showQr = false;
        showNailsRules = false;
        showConsentForm = false;
        paymentIntentId = null;
        qrImageUrl = null;
        showSuccess = true;
    }

    private void ResetCheckoutState()
    {
        showSuccess = false;
        showForm = false;
        showQr = false;
        showNailsRules = false;
        showConsentForm = false;
        paymentIntentId = null;
        qrImageUrl = null;
        consumerError = null;
        nailsRulesAccepted = false;
        consentAccepted = false;
    }

    public void Dispose()
    {
        pollCts?.Cancel();
        pollCts?.Dispose();
    }
}