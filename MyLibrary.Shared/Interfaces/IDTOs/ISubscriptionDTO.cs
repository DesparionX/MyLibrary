using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Shared.Interfaces.IDTOs
{
    public interface ISubscriptionDTO : IDTO<ISubscription<string>, string>, ISubscription<string>
    {
    }
}
