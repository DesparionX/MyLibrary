namespace MyLibrary.Server.Http.Requests
{
    public interface ISubscriptionRequest
    {
        public string? UserId { get; set; }
        public int? SubscriptionTierId { get; set; }

    }
}
