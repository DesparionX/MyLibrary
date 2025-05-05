using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class SubscriptionHandler : ISubscriptionHandler<User>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SubscriptionHandler> _logger;
        private readonly IMapper _mapper;

        public SubscriptionHandler(AppDbContext context, UserManager<User> userManager, ILogger<SubscriptionHandler> logger, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ITaskResult> GetAllTiersAsync()
        {
            try
            {
                var subscriptionTiers = await _context.SubscriptionTiers.ToListAsync();
                if (subscriptionTiers == null || subscriptionTiers.Count == 0)
                {
                    _logger.LogWarning("No tier tiers found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "There are no tiers.", statusCode: StatusCodes.Status404NotFound);
                }

                var dtos = _mapper.Map<ICollection<SubscriptionTierDTO>>(subscriptionTiers);
                return new SubscriptionTaskResult(succeeded: true, message: "SubscriptionTiers retrieved successfully.", statusCode: StatusCodes.Status200OK, tierDtos: dtos);
            }
            catch(Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error retrieving tier tiers.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> CreateSubscriptionAsync(ISubscriptionTierDTO subscriptionDto)
        {
            try
            {
                var newSubscriptionTier = _mapper.Map<SubscriptionTier>(subscriptionDto);
                _context.SubscriptionTiers.Add(newSubscriptionTier);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    _logger.LogInformation("Subscription created successfully.");
                    return new SubscriptionTaskResult(succeeded: true, message: "Subscription created successfully.", statusCode: StatusCodes.Status201Created);
                }

                _logger.LogError("Failed to create tier.");
                return new SubscriptionTaskResult(succeeded: false, message: "Failed to create tier.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error creating tier.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> DeleteSubscriptionAsync(int id)
        {
            try
            {
                var subscriptionToDelete = await _context.SubscriptionTiers.FindAsync(id);
                if (subscriptionToDelete == null)
                {
                    _logger.LogWarning("Subscription not found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "Subscription not found.", statusCode: StatusCodes.Status404NotFound);
                }
                _context.Remove(subscriptionToDelete);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    _logger.LogInformation("Subscription deleted successfully.");
                    return new SubscriptionTaskResult(succeeded: true, message: "Subscription deleted successfully.", statusCode: StatusCodes.Status200OK);
                }

                _logger.LogError("Failed to delete tier.");
                return new SubscriptionTaskResult(succeeded: false, message: "Failed to delete tier.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error deleting tier.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> UpdateSubscriptionAsync(ISubscriptionTierDTO subscriptionTier)
        {
            try
            {
                var sub = await _context.SubscriptionTiers.SingleOrDefaultAsync(s => s.Id == subscriptionTier.Id);
                if (sub == null)
                {
                    _logger.LogWarning("Subscription not found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "Subscription not found.", statusCode: StatusCodes.Status404NotFound);
                }

                // Convert the DTO to entity in case of different property names.
                var convertedSub = _mapper.Map<SubscriptionTier>(subscriptionTier);

                // Ensure the ID remains the same
                convertedSub.Id = sub.Id;

                _context.Entry(sub).CurrentValues.SetValues(convertedSub);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    _logger.LogInformation("Subscription updated successfully.");
                    return new SubscriptionTaskResult(succeeded: true, message: "Subscription updated successfully.", statusCode: StatusCodes.Status200OK);
                }

                _logger.LogError("Failed to update tier.");
                return new SubscriptionTaskResult(succeeded: false, message: "Failed to update tier.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error updating tier.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        public async Task<ITaskResult> GetAllSubscriptionsAsync()
        {
            try
            {
                var subscriptions = await _context.Subscriptions.ToListAsync();
                if (subscriptions == null || subscriptions.Count == 0)
                {
                    _logger.LogWarning("No subscriptions found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "No subscriptions found.", statusCode: StatusCodes.Status404NotFound);
                }

                _logger.LogInformation("Subscriptions retrieved successfully.");
                var subscriptionDtos = _mapper.Map<ICollection<SubscriptionDTO>>(subscriptions);

                return new SubscriptionTaskResult(succeeded: true, message: "Subscription retrieved successfully.", statusCode: StatusCodes.Status200OK, subscriptions: subscriptionDtos);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error retrieving subscriptions.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> FindSubscriptionAsync(string userId)
        {
            try
            {
                var subscription = await _context.Subscriptions
                    .Where(s => s.UserId == userId)
                    .SingleOrDefaultAsync();
                if (subscription == null)
                {
                    _logger.LogWarning("Subscription not found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "Subscription not found.", statusCode: StatusCodes.Status404NotFound);
                }
                var subscriptionDto = _mapper.Map<SubscriptionDTO>(subscription);
                _logger.LogInformation("Subscription retrieved successfully.");
                return new SubscriptionTaskResult(succeeded: true, message: "Subscription retrieved successfully.", statusCode: StatusCodes.Status200OK, subscriptionDto: subscriptionDto);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error retrieving tier.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> SubscribeAsync(ISubscriptionRequest request)
        {
            try
            {
                // Check if user is already subscribed.
                if (await UserAlreadySubscribed(request.UserId!))
                {
                    _logger.LogWarning("User already subscribed.");
                    return new SubscriptionTaskResult(succeeded: false, message: "User already subscribed.", statusCode: StatusCodes.Status400BadRequest);
                }

                // Safely retrieve the subscription tier.
                var tier = await GetSubscription((int)request.SubscriptionTierId!);
                if (tier == null)
                {
                    _logger.LogWarning("Subscription tier not found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "Subscription tier not found.", statusCode: StatusCodes.Status404NotFound);
                }

                // Create and add new subscription.
                var newSubscription = new Subscription
                {
                    UserId = request.UserId,
                    SubscriptionId = request.SubscriptionTierId,
                    SubscriptionTier = tier.Tier,
                    Months = tier.Months,
                    ExpireDate = DateTime.Now.AddMonths((int)tier.Months!)
                };
                _context.Subscriptions.Add(newSubscription);

                // Safely apply subscription benefits and update DB.
                var benefitsApplied = await ApplySubscriptionBenefits(request.UserId!, tier);
                if (benefitsApplied)
                {
                    // ApplySubscriptionBenefits() is already calling the SaveChangesAsync() method from the UserManager.
                    // But I'll keep this here if something changes.

                    // await _context.SaveChangesAsync();

                    _logger.LogInformation("User subscribed successfully.");
                    return new SubscriptionTaskResult(succeeded: true, message: "User subscribed successfully.", statusCode: StatusCodes.Status201Created);

                }

                _logger.LogError("Failed to save subscription and apply benefits.");
                return new SubscriptionTaskResult(succeeded: false, message: "Failed to save subscription and apply benefits.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error subscribing user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> UnsubscribeAsync(ISubscriptionRequest request)
        {
            try
            {
                var subscription = await _context.Subscriptions
                    .Where(s => s.UserId == request.UserId && s.SubscriptionId == request.SubscriptionTierId)
                    .SingleOrDefaultAsync();
                if (subscription == null)
                {
                    _logger.LogWarning("Subscription not found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "Subscription not found.", statusCode: StatusCodes.Status404NotFound);
                }

                _context.Subscriptions.Remove(subscription);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    _logger.LogInformation("User unsubscribed successfully.");
                    return new SubscriptionTaskResult(succeeded: true, message: "User unsubscribed successfully.", statusCode: StatusCodes.Status200OK);
                }

                _logger.LogError("Failed to unsubscribe user.");
                return new SubscriptionTaskResult(succeeded: false, message: "Failed to unsubscribe user.", statusCode: StatusCodes.Status400BadRequest);

            }
            catch(Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error unsubscribing user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> RenewSubscriptionAsync(ISubscriptionRequest request)
        {
            try
            {
                var subscription = await _context.Subscriptions
                    .Where(s => s.UserId == request.UserId && s.SubscriptionId == request.SubscriptionTierId)
                    .SingleOrDefaultAsync();

                var newTier = await _context.SubscriptionTiers
                    .Where(s => s.Id == request.SubscriptionTierId)
                    .SingleOrDefaultAsync();

                if (subscription == null || newTier == null)
                {
                    _logger.LogWarning("Subscription or tier not found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "Subscription or tier not found.", statusCode: StatusCodes.Status404NotFound);
                }

                subscription.SubscriptionId = newTier.Id;
                subscription.SubscriptionTier = newTier.Tier;
                subscription.Months += newTier.Months;
                subscription.ExpireDate!.Value.AddMonths((int)newTier.Months!);

                _context.Update(subscription);
                var result = await _context.SaveChangesAsync();
                if(result > 0)
                {
                    _logger.LogInformation("Subscription renewed successfully.");
                    return new SubscriptionTaskResult(succeeded: true, message: "Subscription renewed successfully.", statusCode: StatusCodes.Status200OK);
                }

                _logger.LogError("Failed to renew tier.");
                return new SubscriptionTaskResult(succeeded: false, message: "Failed to renew tier.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error renewing tier.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> ChangeSubscriptionAsync(ISubscriptionRequest request)
        {
            try
            {
                var subscription = await _context.Subscriptions
                .Where(s => s.UserId == request.UserId)
                .SingleOrDefaultAsync();

                var tier = await _context.SubscriptionTiers
                    .Where(s => s.Id == request.SubscriptionTierId)
                    .SingleOrDefaultAsync();

                if (subscription == null || tier == null)
                {
                    _logger.LogWarning("Subscription or tier not found.");
                    return new SubscriptionTaskResult(succeeded: false, message: "Subscription or tier not found.", statusCode: StatusCodes.Status404NotFound);
                }

                subscription.SubscriptionId = tier.Id;
                subscription.SubscriptionTier = tier.Tier;
                subscription.Months = tier.Months;
                subscription.ExpireDate = DateTime.Now.AddMonths((int)tier.Months!);

                _context.Update(subscription);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    _logger.LogInformation("Subscription changed successfully.");
                    return new SubscriptionTaskResult(succeeded: true, message: "Subscription changed successfully.", statusCode: StatusCodes.Status200OK);
                }

                _logger.LogError("Failed to change tier.");
                return new SubscriptionTaskResult(succeeded: false, message: "Failed to change tier.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch(Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return new SubscriptionTaskResult(succeeded: false, message: "Error changing tier.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        


        // Helper methods
        private async Task<bool> UserAlreadySubscribed(string userId)
        {
            try
            {
                var subscription = await _context.Subscriptions
                    .Where(s => s.UserId == userId)
                    .SingleOrDefaultAsync();

                return subscription != null;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return true;
            }
        }
        private async Task<ISubscriptionTierDTO> GetSubscription(int id)
        {
            try
            {
                var tier = await _context.SubscriptionTiers
                    .Where(s => s.Id == id)
                    .SingleOrDefaultAsync();

                if (tier == null)
                {
                    _logger.LogWarning("Tier not found.");
                    return null;
                }

                var tierDto = _mapper.Map<SubscriptionTierDTO>(tier);
                return tierDto;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return null;
            }
        }
        private async Task<bool> ApplySubscriptionBenefits(string userId, ISubscriptionTier<int> tier)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user == null)
                {
                    _logger.LogWarning("User not found.");
                    return false;
                }
                user.CanBorrow = (bool)tier.CanBorrow!;
                user.BorrowLimit = (int)tier.BorrowLimit!;
                user.Discount = (float)tier.Discount!;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch(Exception err)
            {
                _logger.LogError(err, "[ERROR].\n{0}\n{1}", err.Message, err.StackTrace);
                return false;
            }
        }
    }
}
