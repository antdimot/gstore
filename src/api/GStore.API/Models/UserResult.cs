using GStore.Core.Domain;
using System;
namespace GStore.API.Models
{
    public class UserResult
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool Enabled { get; set; }

        public static UserResult Create( User user )
        {
            return new UserResult {
                Id = user.Id.ToString(),
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Email = user.Email,
                Enabled = user.Enabled
            };
        }
    }
}
