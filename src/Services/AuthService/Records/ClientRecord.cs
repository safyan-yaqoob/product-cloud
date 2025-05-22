using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Records
{
    public record ClientRecord
    {
        [Required(ErrorMessage = "Client ID is required")]
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Redirect URI is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string RedirectUri { get; set; }
        [Required(ErrorMessage = "Post Logout Redirect URIs are required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string PostLogoutRedirectUris { get; set; }
    }

    public record ClientsSummaryRecord
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
    }

    public record EditClientRecord
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUris { get; set; }
    }
}
