using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public interface IItemEvents
    {
        public void OnItemAdded(IItemOperationEvent e);
        public void OnItemRemoved(IItemOperationEvent e);
        public void OnItemSold(IItemOperationEvent e);
    }
}
