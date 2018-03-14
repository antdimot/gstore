using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GStore.Core
{
    public class HashUtility
    {
        const int saltSize = 128 / 8;
        const int outputKeySize = 128 / 8;
        const int numberOfIterations = 100000;

        public HashUtility() { }

        public string MakeHash( string text, string saltText )
        {
            byte[] salt = Convert.FromBase64String( saltText );

            return MakeHash( text, salt );
        }

        public string MakeHash( string text, byte[] salt = null )
        {
            // if the salt doesn't exists create it on the fly
            if( salt == null ) salt = HashUtility.CreateSalt();

            var derivedKey = KeyDerivation.Pbkdf2( text, salt, KeyDerivationPrf.HMACSHA256, numberOfIterations, outputKeySize );

            return Convert.ToBase64String( derivedKey );
        }

        public static byte[] CreateSalt()
        {
            var salt = new byte[saltSize];

            using( var rng = RandomNumberGenerator.Create() )
            {
                rng.GetBytes( salt );

                return salt;
            }
        }

        public static byte[] CreateSalt( string saltText )
        {
            return Convert.FromBase64String( saltText );
        }
    }
}
