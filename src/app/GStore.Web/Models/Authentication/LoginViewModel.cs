using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GStore.Web.Models.Authentication
{
    public class LoginViewModel
    {
        [Display( Name = "Username" )]
        public string Username { get; set; }

        [Display( Name = "Password" )]
        [DataType( DataType.Password )]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
