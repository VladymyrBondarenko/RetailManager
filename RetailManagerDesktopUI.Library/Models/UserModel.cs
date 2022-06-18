using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string EmailAddress { get; set; }

        public Dictionary<string, string> UserRoles { get; set; } = new Dictionary<string, string>();

        public string RoleList =>
            string.Join(", ", UserRoles.Select(x => x.Value));
    }
}
