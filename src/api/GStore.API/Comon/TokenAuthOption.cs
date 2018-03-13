using Microsoft.IdentityModel.Tokens;

namespace GStore.API.Comon
{
    public class TokenAuthOption
    {
        public static string Audience { get; } = "GStoreAudience";
        public static string Issuer { get; } = "GStoreIssuer";

        //public static RsaSecurityKey Key { get; } = new RsaSecurityKey( RSAKeyHelper.GenerateKey() );
        //public static SigningCredentials SigningCredentials { get; } = new SigningCredentials( Key, SecurityAlgorithms.RsaSha256Signature );

        public static string TokenType { get; } = "Bearer";

        public static SymmetricSecurityKey CreateSecurityKey( string appKey )
        {
            return new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey( System.Text.Encoding.Default.GetBytes( appKey ) );
        }
    }
}
