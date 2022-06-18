using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMDataManager.Models
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }

        public string EmailAddress { get; set; }

        public Dictionary<string, string> UserRoles { get; set; }
    }
}