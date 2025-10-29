// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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