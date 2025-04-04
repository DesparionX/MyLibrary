using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public interface IItemEvents
    {
        public Task OnItemAdded(ItemAddedEvent e);
        public void OnItemRemoved(IItemOperationEvent e);
        public void OnItemSold(IItemOperationEvent e);
    }
}
