using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Records
{
    public record ScopeRecord
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; }
        
        [Required(ErrorMessage = "Resources are required")]
        public string Resources { get; set; }
    }

    public record ScopeSummaryRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
    
    public record EditScopeRecord
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; }
        
        [Required(ErrorMessage = "Resources are required")]
        public string Resources { get; set; }
    }
}
