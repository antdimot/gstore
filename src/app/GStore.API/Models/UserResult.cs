using GStore.Core.Domain;
namespace GStore.API.Models
{
    public class UserResult
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }

        public bool Enabled { get; set; }

        public static UserResult Create( User user )
        {
            return new UserResult {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Enabled = user.Enabled
            };
        }
    }
}
