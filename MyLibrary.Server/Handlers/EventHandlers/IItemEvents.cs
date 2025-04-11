using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public interface IItemEvents
    {
        public Task OnItemAdded(IItemOperationEvent e);
        public Task OnItemUpdated(IItemOperationEvent e);
        public Task OnItemRemoved(IItemOperationEvent e);
        public Task OnItemSold(IItemOperationEvent e);
    }
}
