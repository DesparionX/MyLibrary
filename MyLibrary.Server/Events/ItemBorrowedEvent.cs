namespace MyLibrary.Server.Events
{
    public class ItemBorrowedEvent : IItemOperationEvent
    {
        public string ISBN { get; }

        public string Name { get; }

        public int Quantity { get; }

        public ItemBorrowedEvent(string isbn, string name, int quantity)
        {
            ISBN = isbn;
            Name = name;
            Quantity = quantity;
        }
    }
}
