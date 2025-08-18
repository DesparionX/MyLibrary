using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Data.DTOs
{
    public class SubscriptionDTO : ISubscriptionDTO
    {
        public required string Id { get; set; }
        public int? SubscriptionId { get; set; }
        public string? SubscriptionTier { get; set; }
        public string? UserId { get; set; }
        public int? Months { get; set; }
        public DateTime? ExpireDate { get; set; }
        
    }
}
