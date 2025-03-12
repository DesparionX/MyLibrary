namespace MyLibrary.Server.Events
{
    public interface IItemOperationEvent
    {
        public string ISBN { get; }
        public string Name { get; }
        public int Quantity { get; }
    }
}
