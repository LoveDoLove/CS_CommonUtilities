// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Stripe;
using Stripe.Checkout;

namespace CommonUtilities.Helpers.Stripe;

/// <summary>
///     Defines methods for interacting with the Stripe API for payment processing, customer management, and related
///     operations.
/// </summary>
public interface IStripeHelper
{
    /// <summary>
    ///     Asynchronously creates a new tax rate in Stripe.
    /// </summary>
    /// <param name="taxRateCreateOptions">The options for creating the tax rate.</param>
    /// <returns>The created <see cref="TaxRate" />, or null if creation failed.</returns>
    Task<TaxRate?> CreateTaxRateAsync(TaxRateCreateOptions taxRateCreateOptions);

    /// <summary>
    ///     Asynchronously creates a new customer in Stripe.
    /// </summary>
    /// <param name="customerCreateOptions">The options for creating the customer.</param>
    /// <returns>The created <see cref="Customer" />, or null if creation failed.</returns>
    Task<Customer?> CreateCustomerAsync(CustomerCreateOptions customerCreateOptions);

    /// <summary>
    ///     Asynchronously creates a new checkout session in Stripe.
    /// </summary>
    /// <param name="sessionCreateOptions">The options for creating the checkout session.</param>
    /// <returns>The created <see cref="Session" />, or null if creation failed.</returns>
    Task<Session?> CreateCheckoutSessionAsync(SessionCreateOptions sessionCreateOptions);

    /// <summary>
    ///     Asynchronously retrieves a checkout session by its ID.
    /// </summary>
    /// <param name="sessionId">The ID of the checkout session.</param>
    /// <returns>The <see cref="Session" /> if found; otherwise, null.</returns>
    Task<Session?> GetCheckoutSessionAsync(string sessionId);

    /// <summary>
    ///     Asynchronously retrieves a payment intent by its ID.
    /// </summary>
    /// <param name="paymentIntentId">The ID of the payment intent.</param>
    /// <returns>The <see cref="PaymentIntent" /> if found; otherwise, null.</returns>
    Task<PaymentIntent?> GetPaymentIntentAsync(string paymentIntentId);

    /// <summary>
    ///     Asynchronously retrieves a charge by its ID.
    /// </summary>
    /// <param name="chargeId">The ID of the charge.</param>
    /// <returns>The <see cref="Charge" /> if found; otherwise, null.</returns>
    Task<Charge?> GetChargeAsync(string chargeId);

    /// <summary>
    ///     Asynchronously retrieves a customer by its ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>The <see cref="Customer" /> if found; otherwise, null.</returns>
    Task<Customer?> GetCustomerAsync(string customerId);

    /// <summary>
    ///     Asynchronously refunds a payment intent for the specified amount.
    /// </summary>
    /// <param name="paymentIntentId">The ID of the payment intent to refund.</param>
    /// <param name="amount">The amount to refund.</param>
    /// <returns>The <see cref="Refund" /> if successful; otherwise, null.</returns>
    Task<Refund?> RefundPaymentIntentAsync(string paymentIntentId, decimal amount);
}