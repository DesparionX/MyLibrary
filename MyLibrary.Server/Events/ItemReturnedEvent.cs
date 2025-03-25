namespace MyLibrary.Server.Events
{
    public class ItemReturnedEvent : IItemOperationEvent
    {
        public string ISBN { get; }

        public string Name { get; }

        public int Quantity { get; }

        public ItemReturnedEvent(string isbn, string name, int quantity)
        {
            ISBN = isbn;
            Name = name;
            Quantity = quantity;
        }
    }
}
