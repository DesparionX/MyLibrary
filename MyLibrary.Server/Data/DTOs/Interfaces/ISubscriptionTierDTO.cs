using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.DTOs.Interfaces
{
    public interface ISubscriptionTierDTO : IDTO<ISubscriptionTier<int>, int>, ISubscriptionTier<int>
    {
    }
}
