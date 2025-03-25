namespace MyLibrary.Server.Events
{
    public class ItemSoldEvent : IItemOperationEvent
    {
        public string ISBN { get; }

        public string Name { get; }

        public int Quantity { get; }

        public ItemSoldEvent(string isbn, string name, int quantity)
        {
            ISBN = isbn;
            Name = name;
            Quantity = quantity;
        }
    }
}
