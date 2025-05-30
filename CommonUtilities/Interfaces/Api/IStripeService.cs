using Stripe;
using Stripe.Checkout;

namespace CommonUtilities.Interfaces.Api;

/// <summary>
///     Defines the contract for a service that interacts with the Stripe API
///     for payment processing, customer management, and other Stripe-related operations.
/// </summary>
public interface IStripeService
{
    /// <summary>
    ///     Asynchronously creates a new tax rate in Stripe.
    /// </summary>
    /// <param name="taxRateCreateOptions">The options for creating the tax rate.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created <see cref="TaxRate" />
    ///     object if successful;
    ///     otherwise, <c>null</c> if creation failed.
    /// </returns>
    Task<TaxRate?> CreateTaxRateAsync(TaxRateCreateOptions taxRateCreateOptions);

    /// <summary>
    ///     Asynchronously creates a new customer in Stripe.
    /// </summary>
    /// <param name="customerCreateOptions">The options for creating the customer.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created <see cref="Customer" />
    ///     object if successful;
    ///     otherwise, <c>null</c> if creation failed.
    /// </returns>
    Task<Customer?> CreateCustomerAsync(CustomerCreateOptions customerCreateOptions);

    /// <summary>
    ///     Asynchronously creates a new Stripe Checkout session.
    /// </summary>
    /// <param name="sessionCreateOptions">The options for creating the Checkout session.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created <see cref="Session" />
    ///     object if successful;
    ///     otherwise, <c>null</c> if creation failed.
    /// </returns>
    Task<Session?> CreateCheckoutSessionAsync(SessionCreateOptions sessionCreateOptions);

    /// <summary>
    ///     Asynchronously retrieves an existing Stripe Checkout session by its ID.
    /// </summary>
    /// <param name="sessionId">The ID of the Checkout session to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="Session" /> object if
    ///     found;
    ///     otherwise, <c>null</c>.
    /// </returns>
    Task<Session?> GetCheckoutSessionAsync(string sessionId);

    /// <summary>
    ///     Asynchronously retrieves an existing Stripe PaymentIntent by its ID.
    /// </summary>
    /// <param name="paymentIntentId">The ID of the PaymentIntent to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="PaymentIntent" /> object
    ///     if found;
    ///     otherwise, <c>null</c>.
    /// </returns>
    Task<PaymentIntent?> GetPaymentIntentAsync(string paymentIntentId);

    /// <summary>
    ///     Asynchronously retrieves an existing Stripe Charge by its ID.
    /// </summary>
    /// <param name="chargeId">The ID of the Charge to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="Charge" /> object if
    ///     found;
    ///     otherwise, <c>null</c>.
    /// </returns>
    Task<Charge?> GetChargeAsync(string chargeId);

    /// <summary>
    ///     Asynchronously retrieves an existing Stripe Customer by their ID.
    /// </summary>
    /// <param name="customerId">The ID of the Customer to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="Customer" /> object if
    ///     found;
    ///     otherwise, <c>null</c>.
    /// </returns>
    Task<Customer?> GetCustomerAsync(string customerId);

    /// <summary>
    ///     Asynchronously refunds a payment associated with a specific PaymentIntent.
    /// </summary>
    /// <param name="paymentIntentId">The ID of the PaymentIntent to refund.</param>
    /// <param name="amount">
    ///     The amount to refund. If null, a full refund is attempted. Note: Stripe's API might require
    ///     specific handling for partial vs full refunds.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="Refund" /> object if the
    ///     refund was successful;
    ///     otherwise, <c>null</c>.
    /// </returns>
    Task<Refund?>
        RefundPaymentIntentAsync(string paymentIntentId,
            decimal amount); // Consider if 'amount' should be nullable for full refunds, or if specific options object is better.
}