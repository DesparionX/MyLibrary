using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ISubscriptionHandler<User> _subscriptionHandler;
        private readonly IResultHandler<ITaskResult> _resultHandler;
        public AdminController(ISubscriptionHandler<User> subscriptionHandler, IResultHandler<ITaskResult> resultHandler)
        {
            _subscriptionHandler = subscriptionHandler;
            _resultHandler = resultHandler;
        }

        [HttpGet("subscriptions/getAllTiers")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubscriptionTiers()
        {
            var response = await _subscriptionHandler.GetAllTiersAsync();
            return _resultHandler.ReadResult(response);
        }

        [HttpPost("subscriptions/registerTier")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status201Created)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterSubscriptionTier([FromBody] SubscriptionTierDTO subscriptionDto)
        {
            if(subscriptionDto == null)
            {
                return BadRequest(new SubscriptionTaskResult(succeeded: false, message: "Request cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var response = await _subscriptionHandler.CreateSubscriptionAsync(subscriptionDto);
            return _resultHandler.ReadResult(response);
        }

        [HttpDelete("subscriptions/deleteTier")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSubscriptionTier([FromBody] int id)
        {
            var response = await _subscriptionHandler.DeleteSubscriptionAsync(id);
            return _resultHandler.ReadResult(response);
        }

        [HttpPut("subscriptions/updateTier")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubscriptionTier([FromBody] SubscriptionTierDTO subscriptionDto)
        {
            if (subscriptionDto == null)
            {
                return BadRequest(new SubscriptionTaskResult(succeeded: false, message: "Request cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var response = await _subscriptionHandler.UpdateSubscriptionAsync(subscriptionDto);
            return _resultHandler.ReadResult(response);
        }

        [HttpGet("subscriptions/getAllSubscriptions")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var response = await _subscriptionHandler.GetAllSubscriptionsAsync();
            return _resultHandler.ReadResult(response);
        }

        [HttpGet("subscriptions/findSubscription")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindSubscription([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new SubscriptionTaskResult(succeeded: false, message: "Request cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var response = await _subscriptionHandler.FindSubscriptionAsync(userId);
            return _resultHandler.ReadResult(response);
        }

        [HttpPost("subscriptions/subscribe")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new SubscriptionTaskResult(succeeded: false, message: "Request cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var response = await _subscriptionHandler.SubscribeAsync(request);
            return _resultHandler.ReadResult(response);
        }

        [HttpDelete("subscriptions/unsubscribe")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Unsubscribe([FromBody] SubscriptionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new SubscriptionTaskResult(succeeded: false, message: "Request cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var response = await _subscriptionHandler.UnsubscribeAsync(request);
            return _resultHandler.ReadResult(response);
        }

        [HttpPut("subscriptions/renew")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RenewSubscription([FromBody] SubscriptionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new SubscriptionTaskResult(succeeded: false, message: "Request cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var response = await _subscriptionHandler.RenewSubscriptionAsync(request);
            return _resultHandler.ReadResult(response);
        }

        [HttpPut("subscriptions/changeSubscription")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeSubscription([FromBody] SubscriptionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new SubscriptionTaskResult(succeeded: false, message: "Request cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var response = await _subscriptionHandler.ChangeSubscriptionAsync(request);
            return _resultHandler.ReadResult(response);
        }

    }
}
