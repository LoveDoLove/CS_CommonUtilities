using CommonUtilities.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace CommonUtilities.Services;

/// <summary>
/// Service for interacting with the Stripe API.
/// </summary>
public class StripeService : IStripeService
{
    /// <summary>
    /// The StripeClient instance used for making API calls.
    /// </summary>
    public readonly StripeClient StripeClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="StripeService"/> class.
    /// </summary>
    /// <param name="stripeApp">The Stripe application configuration containing the API key.</param>
    /// <exception cref="ArgumentException">Thrown if the Stripe API key is not configured.</exception>
    public StripeService(StripeApp stripeApp)
    {
        if (string.IsNullOrEmpty(stripeApp.ApiKey))
            throw new ArgumentException("Stripe API key is not configured. Please set a valid API key.");

        StripeClient = new StripeClient(stripeApp.ApiKey);
    }

    /// <summary>
    /// Creates a new tax rate asynchronously.
    /// </summary>
    /// <param name="taxRateCreateOptions">The options for creating the tax rate.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the created <see cref="TaxRate"/>, or null if creation failed.</returns>
    public async Task<TaxRate?> CreateTaxRateAsync(TaxRateCreateOptions taxRateCreateOptions)
    {
        TaxRateService taxRateService = new(StripeClient);
        TaxRate? taxRate = await taxRateService.CreateAsync(taxRateCreateOptions);
        // Consider throwing an exception if taxRate is null and a tax rate is always expected
        // if (taxRate == null) throw new Exception("Failed to create tax rate.");
        return taxRate;
    }

    /// <summary>
    /// Creates a new customer asynchronously.
    /// </summary>
    /// <param name="customerCreateOptions">The options for creating the customer.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the created <see cref="Customer"/>, or null if creation failed.</returns>
    public async Task<Customer?> CreateCustomerAsync(CustomerCreateOptions customerCreateOptions)
    {
        CustomerService customerService = new(StripeClient);
        Customer? customer = await customerService.CreateAsync(customerCreateOptions);
        return customer;
    }

    /// <summary>
    /// Creates a new checkout session asynchronously.
    /// </summary>
    /// <param name="sessionCreateOptions">The options for creating the checkout session.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the created <see cref="Session"/>, or null if creation failed.</returns>
    public async Task<Session?> CreateCheckoutSessionAsync(SessionCreateOptions sessionCreateOptions)
    {
        SessionService sessionService = new(StripeClient);
        Session? session = await sessionService.CreateAsync(sessionCreateOptions);
        return session;
    }

    /// <summary>
    /// Retrieves a checkout session asynchronously.
    /// </summary>
    /// <param name="sessionId">The ID of the checkout session to retrieve.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the retrieved <see cref="Session"/>, or null if not found.</returns>
    public async Task<Session?> GetCheckoutSessionAsync(string sessionId)
    {
        SessionService sessionService = new(StripeClient);
        Session? session = await sessionService.GetAsync(sessionId);
        return session;
    }

    /// <summary>
    /// Retrieves a payment intent asynchronously.
    /// </summary>
    /// <param name="paymentIntentId">The ID of the payment intent to retrieve.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the retrieved <see cref="PaymentIntent"/>, or null if not found.</returns>
    public async Task<PaymentIntent?> GetPaymentIntentAsync(string paymentIntentId)
    {
        PaymentIntentService paymentIntentService = new(StripeClient);
        PaymentIntent? paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);
        return paymentIntent;
    }

    /// <summary>
    /// Retrieves a charge asynchronously.
    /// </summary>
    /// <param name="chargeId">The ID of the charge to retrieve.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the retrieved <see cref="Charge"/>, or null if not found.</returns>
    public async Task<Charge?> GetChargeAsync(string chargeId)
    {
        ChargeService chargeService = new(StripeClient);
        Charge? charge = await chargeService.GetAsync(chargeId);
        return charge;
    }

    /// <summary>
    /// Retrieves a customer asynchronously.
    /// </summary>
    /// <param name="customerId">The ID of the customer to retrieve.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the retrieved <see cref="Customer"/>, or null if not found.</returns>
    public async Task<Customer?> GetCustomerAsync(string customerId)
    {
        CustomerService customerService = new(StripeClient);
        Customer? customer = await customerService.GetAsync(customerId);
        return customer;
    }

    /// <summary>
    /// Refunds a payment intent asynchronously.
    /// </summary>
    /// <param name="paymentIntentId">The ID of the payment intent to refund.</param>
    /// <param name="amount">The amount to refund.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the created <see cref="Refund"/>, or null if refund failed.</returns>
    public async Task<Refund?> RefundPaymentIntentAsync(string paymentIntentId, decimal amount)
    {
        RefundService refundService = new RefundService(StripeClient);
        RefundCreateOptions refundCreateOptions = new()
        {
            PaymentIntent = paymentIntentId,
            Amount = (long)(amount * 100)
        };
        Refund? refund = await refundService.CreateAsync(refundCreateOptions);
        return refund;
    }
}