using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs.Controllers
{
    public class Account
    {
        public string FindUser { get; set; } = string.Empty;
        public string GetAllUsers { get; set; } = string.Empty;
        public string Register { get; set; } = string.Empty;
        public string UpdateUser { get; set; } = string.Empty;
        public string DeleteUser { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string GetUserIdentity { get; set; } = string.Empty;
        public string LogOut { get; set; } = string.Empty;
    }
}
