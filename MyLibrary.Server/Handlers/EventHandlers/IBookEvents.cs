using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public interface IBookEvents
    {
        
        public void OnBookBorrowed(IItemOperationEvent e);
        public void OnBookReturned(IItemOperationEvent e);
    }
}
