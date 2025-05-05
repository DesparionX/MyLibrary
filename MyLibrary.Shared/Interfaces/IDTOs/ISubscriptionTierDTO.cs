using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Shared.Interfaces.IDTOs
{
    public interface ISubscriptionTierDTO : IDTO<ISubscriptionTier<int>, int>, ISubscriptionTier<int>
    {
    }
}
