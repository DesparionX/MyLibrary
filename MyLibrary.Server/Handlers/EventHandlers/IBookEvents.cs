using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public interface IBookEvents
    {
        public void OnBookAdded(IItemOperationEvent e);
        public void OnBookUpdated(IItemOperationEvent e);
        public void OnBookDeleted(IItemOperationEvent e);
        public void OnBookBorrowed(IItemOperationEvent e);
        public void OnBookReturned(IItemOperationEvent e);
    }
}
