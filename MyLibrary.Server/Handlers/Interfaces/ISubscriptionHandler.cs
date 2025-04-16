using Microsoft.AspNetCore.Identity;
using MyLibrary.Server.Data.DTOs.Interfaces;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers.Interfaces
{
    public interface ISubscriptionHandler<TUser> where TUser : IdentityUser<string>
    {
        // Admin methods.
        public Task<ITaskResult> GetAllTiersAsync();
        public Task<ITaskResult> CreateSubscriptionAsync(ISubscriptionTierDTO subscriptionDto);
        public Task<ITaskResult> DeleteSubscriptionAsync(int id);
        public Task<ITaskResult> UpdateSubscriptionAsync(ISubscriptionTierDTO subscriptionTier);
        

        // User methods.
        public Task<ITaskResult> GetAllSubscriptionsAsync();
        public Task<ITaskResult> FindSubscriptionAsync(string userId);
        public Task<ITaskResult> SubscribeAsync(ISubscriptionRequest request);
        public Task<ITaskResult> UnsubscribeAsync(ISubscriptionRequest request);
        public Task<ITaskResult> RenewSubscriptionAsync(ISubscriptionRequest request);
        public Task<ITaskResult> ChangeSubscriptionAsync(ISubscriptionRequest request);
    }
}
