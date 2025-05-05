using MyLibrary.Server.Data.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Http.Responses
{
    public class SubscriptionTaskResult : ITaskResult
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }
        public SubscriptionTierDTO? SubscriptionTier { get; }
        public SubscriptionDTO? Subscription { get; }

        public ICollection<SubscriptionTierDTO>? SubscriptionTiers { get; }
        public ICollection<SubscriptionDTO>? Subscriptions { get; set; }

        public SubscriptionTaskResult(bool succeeded, int statusCode, 
            string? message = "", 
            SubscriptionTierDTO? tierDto = null,
            SubscriptionDTO? subscriptionDto = null,
            ICollection<SubscriptionDTO>? subscriptions = null,
            ICollection<SubscriptionTierDTO>? tierDtos = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
            Subscription = subscriptionDto;
            Subscriptions = subscriptions;
            SubscriptionTier = tierDto;
            SubscriptionTiers = tierDtos;
        }
    }
}
