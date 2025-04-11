﻿namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface ISubscription : IEntity<string>
    {
        public int? SubscriptionId { get; set; }
        public string? SubscriptionTier { get; set; }
        public string? UserId { get; set; }
        public int? Months { get; set; }
        public DateTime? ExpireDate { get; set; }

    }
}
