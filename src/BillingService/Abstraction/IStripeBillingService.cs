using BillingService.Shared.Models;
using Stripe.Checkout;
using Stripe;

namespace BillingService.Abstraction
{
  public interface IStripeBillingService
  {
    Task<string> CreateCheckoutSessionAsync(StripeCheckoutRequest request);
  }
  public class StripeBillingService : IStripeBillingService
  {
    private readonly IConfiguration _configuration;

    public StripeBillingService(IConfiguration configuration)
    {
      _configuration = configuration;
      StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    public async Task<string> CreateCheckoutSessionAsync(StripeCheckoutRequest request)
    {
      var options = new SessionCreateOptions
      {
        PaymentMethodTypes = new List<string> { "card" },
        Mode = "payment",
        CustomerEmail = request.CustomerEmail,
        LineItems = new List<SessionLineItemOptions>
        {
            new()
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = request.Currency.ToLower(),
                    UnitAmountDecimal = request.Amount * 100, // in cents
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = request.ProductName
                    }
                },
                Quantity = 1
            }
        },
        SuccessUrl = request.SuccessUrl + "?session_id={CHECKOUT_SESSION_ID}",
        CancelUrl = request.CancelUrl
      };

      var service = new SessionService();
      Session session = await service.CreateAsync(options);

      return session.Url!;
    }
  }


}
