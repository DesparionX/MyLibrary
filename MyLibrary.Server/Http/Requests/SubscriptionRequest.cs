namespace MyLibrary.Server.Http.Requests
{
    public class SubscriptionRequest : ISubscriptionRequest
    {
        public string? UserId { get; set; }
        public int? SubscriptionTierId { get; set; }
    }
}
