using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace IdentityServer.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public ErrorModel(ILogger<ErrorModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
        public string ErrorMessage { get; set; }
        public string ErrorTitle { get; set; }
        public bool ShowErrorDetails => _environment.IsDevelopment();
        public Exception Exception { get; set; }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                Exception = exceptionFeature.Error;
                ErrorMessage = GetUserFriendlyErrorMessage(Exception);
                ErrorTitle = GetErrorTitle(Exception);
                
                _logger.LogError(Exception, "An unhandled exception occurred. RequestId: {RequestId}", RequestId);
            }
            else
            {
                ErrorTitle = "Error";
                ErrorMessage = "An error occurred while processing your request.";
            }
        }

        private string GetUserFriendlyErrorMessage(Exception exception)
        {
            // Add custom error messages for specific exceptions
            return exception switch
            {
                UnauthorizedAccessException _ => "You don't have permission to access this resource.",
                InvalidOperationException _ => "The requested operation could not be completed.",
                ArgumentException _ => "Invalid input provided.",
                TimeoutException _ => "The request took too long to process. Please try again.",
                _ => "We encountered an unexpected error. Please try again later."
            };
        }

        private string GetErrorTitle(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException _ => "Access Denied",
                InvalidOperationException _ => "Operation Failed",
                ArgumentException _ => "Invalid Input",
                TimeoutException _ => "Request Timeout",
                _ => "Oops! Something went wrong"
            };
        }
    }

}
