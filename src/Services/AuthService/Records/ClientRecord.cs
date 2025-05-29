using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Records
{
    public record ClientRecord
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Client ID is required")]
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Redirect URI is required")]
        public string RedirectUri { get; set; }
        [Required(ErrorMessage = "Post Logout Redirect URIs are required")]
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
