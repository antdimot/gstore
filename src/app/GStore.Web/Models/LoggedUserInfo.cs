using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GStore.Web.Models
{
    public class LoggedUserInfo
    {
        const string _gstoreUser= "gstore.loggeduserinfo";

        // internal properties
        public string UserId { get; set; }
        public string[] Authorizzations { get; set; }
        public string Username { get; set; }

        // this property is used for showing user on web site
        public string DisplayName { get; set; }

        private static IHttpContextAccessor httpContextAccessor;

        public static void SetHttpContextAccessor( IHttpContextAccessor accessor )
        {
            httpContextAccessor = accessor;
        }

        public static LoggedUserInfo Current
        {
            get
            {
                var value = httpContextAccessor.HttpContext.Session.GetString( _gstoreUser );

                var result = value == null ? default( LoggedUserInfo ) : JsonConvert.DeserializeObject<LoggedUserInfo>( value );

                return result;
            }
            set
            {
                if( value == null )
                {
                    httpContextAccessor.HttpContext.Session.Clear();
                    return;
                }

                httpContextAccessor.HttpContext
                                   .Session
                                   .SetString( _gstoreUser, JsonConvert.SerializeObject( value ) );
            }
        }
    }
}
