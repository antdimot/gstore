using GStore.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GStore.API.Comon
{
    public class Utils
    {
        public class ContentType
        {
            public const string Text = "text/plain";
            public const string Html = "text/html";
        }
    }
}
