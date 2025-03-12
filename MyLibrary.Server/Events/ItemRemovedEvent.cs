namespace MyLibrary.Server.Events
{
    public class ItemRemovedEvent : IItemOperationEvent
    {
        public string ISBN { get; }

        public string Name { get; }

        public int Quantity { get; }

        public ItemRemovedEvent(string isbn, string name, int quantity)
        {
            ISBN = isbn;
            Name = name;
            Quantity = quantity;
        }
    }
}
