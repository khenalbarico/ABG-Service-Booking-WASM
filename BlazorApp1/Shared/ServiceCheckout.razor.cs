using Microsoft.AspNetCore.Components;
using WasmCore1.Algorithms;
using WasmCore1.Models.Client;
using WasmCore1.Models.PolicyForms;
using static WasmCore1.Models.Constants;

namespace BlazorApp1.Shared;

public partial class ServiceCheckout : IDisposable
{
    [Parameter] public ClientRequest         Request { get; set; } = new();
    [Parameter] public EventCallback<string> OnRemove { get; set; }
    [Parameter] public EventCallback         OnClose { get; set; }
    [Parameter] public EventCallback         OnCompleted { get; set; }
    [Parameter] public EventCallback         OnSchedulesChanged { get; set; }

    private const int QrPollingTimeoutSeconds = 120;

    bool showForm;
    bool showSuccess;
    bool showQr;
    bool showNailsRules;
    bool showConsentForm;
    bool isLoading;

    bool nailsRulesAccepted;
    bool consentAccepted;

    string? qrImageUrl;
    string? paymentIntentId;
    CancellationTokenSource? pollCts;

    string? consumerError;
    bool showConfirmModal;

    int qrCountdownSeconds = QrPollingTimeoutSeconds;
    string qrCountdownDisplay => TimeSpan.FromSeconds(qrCountdownSeconds).ToString(@"mm\:ss");

    private async Task Remove(string uid)
        => await OnRemove.InvokeAsync(uid);

    private void OpenForm()
        => showForm = true;

    private void CloseForm()
    {
        consumerError = null;
        showForm      = false;
    }

    private void OpenConfirmModal()
    {
        consumerError    = string.Empty;
        showConfirmModal = true;
    }

    private void CloseConfirmModal()
        => showConfirmModal = false;

    private async Task ConfirmProceed()
    {
        showConfirmModal = false;
        await Submit();
    }

    private decimal GetTotalAmount()
        => CheckoutSummaryAlgorithms.GetTotalAmount(Request);

    private string GetBranchDisplayName(ServiceBranch branch)
        => CheckoutSummaryAlgorithms.GetBranchDisplayName(branch);

    private async Task Submit()
    {
        consumerError = null;

        CheckoutRequestAlgorithms.PrepareClientInformation(Request);

        if (Request.ClientConsent == null)
            Request.ClientConsent = new ConsentModel();

        showForm = false;

        var nextStep = CheckoutPolicyAlgorithms.ResolveNextStep(
            Request,
            nailsRulesAccepted,
            consentAccepted);

        if (nextStep == CheckoutFlowStep.NailsRules)
        {
            showNailsRules  = true;
            showConsentForm = false;
            return;
        }

        if (nextStep == CheckoutFlowStep.ConsentForm)
        {
            showNailsRules  = false;
            showConsentForm = true;
            return;
        }

        await StartPaymentAsync();
    }

    private async Task HandleNailsRulesAccepted()
    {
        nailsRulesAccepted = true;
        showNailsRules     = false;

        var nextStep = CheckoutPolicyAlgorithms.ResolveNextStep(
            Request,
            nailsRulesAccepted,
            consentAccepted);

        if (nextStep == CheckoutFlowStep.ConsentForm)
        {
            if (Request.ClientConsent == null)
                Request.ClientConsent = new ConsentModel();

            showConsentForm = true;
            return;
        }

        await StartPaymentAsync();
    }

    private void BackFromNailsRules()
    {
        showNailsRules = false;
        showForm       = true;
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
        isLoading     = true;
        consumerError = null;
        StateHasChanged();

        try
        {
                         await Db.PostClientRequestAsync(Request);
            var result = await Payment.CreateQrphChargeAsync(Request);

            paymentIntentId = result.PaymentIntentId;
            qrImageUrl      = result.QrImageUrl;

            showQr          = true;
            showForm        = false;
            showNailsRules  = false;
            showConsentForm = false;

            qrCountdownSeconds = QrPollingTimeoutSeconds;

            pollCts?.Cancel();
            pollCts?.Dispose();
            pollCts = new CancellationTokenSource();

            _ = PollPaymentStatus(pollCts.Token);
        }
        finally
        {
            isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task PollPaymentStatus(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested && qrCountdownSeconds > 0)
            {
                if (qrCountdownSeconds == QrPollingTimeoutSeconds || qrCountdownSeconds % 3 == 0)
                {
                    var status = await Payment.ProcessClientPaymentAsync(paymentIntentId!, Request);

                    if (CheckoutPaymentAlgorithms.IsPaymentSuccessful(status))
                    {
                        pollCts?.Cancel();

                        await OnSchedulesChanged.InvokeAsync();

                        ShowSuccessState();

                        await InvokeAsync(StateHasChanged);
                        return;
                    }
                }

                await Task.Delay(1000, ct);
                qrCountdownSeconds--;
                await InvokeAsync(StateHasChanged);
            }

            if (!ct.IsCancellationRequested && qrCountdownSeconds <= 0)
            {
                await InvokeAsync(HandlePaymentExpired);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    private void HandlePaymentExpired()
    {
        pollCts?.Cancel();

        showQr             = false;
        paymentIntentId    = null;
        qrImageUrl         = null;
        qrCountdownSeconds = QrPollingTimeoutSeconds;

        showForm = true;

        consumerError = "The QR code expired after 120 seconds. Please click Proceed again to generate a new QR code.";

        StateHasChanged();
    }

    private void CancelPayment()
    {
        pollCts?.Cancel();
        showQr             = false;
        paymentIntentId    = null;
        qrImageUrl         = null;
        qrCountdownSeconds = QrPollingTimeoutSeconds;
        showForm           = true;
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
        showForm           = false;
        showQr             = false;
        showNailsRules     = false;
        showConsentForm    = false;
        paymentIntentId    = null;
        qrImageUrl         = null;
        qrCountdownSeconds = QrPollingTimeoutSeconds;
        showSuccess        = true;
    }

    private void ResetCheckoutState()
    {
        showSuccess           = false;
        showForm              = false;
        showQr                = false;
        showNailsRules        = false;
        showConsentForm       = false;
        paymentIntentId       = null;
        qrImageUrl            = null;
        qrCountdownSeconds    = QrPollingTimeoutSeconds;
        consumerError         = null;
        nailsRulesAccepted    = false;
        consentAccepted       = false;
        Request.ClientConsent = new ConsentModel();
    }

    public void Dispose()
    {
        pollCts?.Cancel();
        pollCts?.Dispose();
    }
}