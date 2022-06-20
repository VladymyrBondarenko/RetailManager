namespace RetailManagerAPI.Models
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }

        public string EmailAddress { get; set; }

        public Dictionary<string, string> UserRoles { get; set; }
    }
}
