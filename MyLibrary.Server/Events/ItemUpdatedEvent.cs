namespace MyLibrary.Server.Events
{
    public class ItemUpdatedEvent : IItemOperationEvent
    {
        public string ISBN { get; }

        public string Name { get; }

        public int Quantity { get; }

        public ItemUpdatedEvent(string isbn, string name, int quantity = 0)
        {
            ISBN = isbn;
            Name = name;
            Quantity = quantity;
        }
    }
}
