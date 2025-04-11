using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public interface IBookEvents
    {
        
        public Task OnBookBorrowed(IItemOperationEvent e);
        public Task OnBookReturned(IItemOperationEvent e);
    }
}
