namespace MyLibrary.Server.Events
{
    public class ItemAddedEvent : IItemOperationEvent
    {
        public string ISBN { get; }
        public string Name { get; }
        public int Quantity { get; }
        public ItemAddedEvent(string isbn, string name, int quantity)
        {
            ISBN = isbn;
            Name = name;
            Quantity = quantity;
        }
    }
}
