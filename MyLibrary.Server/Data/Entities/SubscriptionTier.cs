﻿using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.Entities
{
    public class SubscriptionTier : ISubscriptionTier<int>
    {
        public int Id { get; set; }
        public string? Tier { get; set; }
        public int? Months { get; set; }
        public decimal? Cost { get; set; }
        public bool? CanBorrow { get; set; }
        public int? BorrowLimit { get; set; }
        public float? Discount { get; set; }

    }
}
