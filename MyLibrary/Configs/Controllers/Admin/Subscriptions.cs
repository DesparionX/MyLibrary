using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs.Controllers.Admin
{
    public class Subscriptions
    {
        public string GetAllTiers { get; set; } = string.Empty;
        public string RegisterTier { get; set; } = string.Empty;
        public string DeleteTier { get; set; } = string.Empty;
        public string UpdateTier { get; set; } = string.Empty;
        public string GetAllSubscriptions { get; set; } = string.Empty;
        public string FindSubscription { get; set; } = string.Empty;
        public string SubscribeUser { get; set; } = string.Empty;
        public string Unsubscribe { get; set; } = string.Empty;
        public string RenewSubscription { get; set; } = string.Empty;
        public string ChangeSubscription { get; set; } = string.Empty;
    }
}
