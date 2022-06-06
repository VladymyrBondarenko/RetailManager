using System;

namespace RMDataManager.Library.Models
{
    public class UserModel
    {
        public string Id { get; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
