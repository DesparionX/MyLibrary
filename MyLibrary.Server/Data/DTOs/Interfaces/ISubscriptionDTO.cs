using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.DTOs.Interfaces
{
    public interface ISubscriptionDTO : IDTO<ISubscription<string>, string>, ISubscription<string>
    {
    }
}
