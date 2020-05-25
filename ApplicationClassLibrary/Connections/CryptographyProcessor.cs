using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ApplicationClassLibrary.Connections
{
    public class CryptographyProcessor
    {

        /// <summary>
        /// Size of salt.
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Size of hash.
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// Creates a hash from a password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password, int iterations)
        {
            // Create salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt;
                rng.GetBytes(salt = new byte[SaltSize]);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    var hash = pbkdf2.GetBytes(HashSize);
                    // Combine salt and hash
                    var hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                    // Convert to base64
                    var base64Hash = Convert.ToBase64String(hashBytes);

                    // Format hash with extra information
                    return $"$HASH|V1${iterations}${base64Hash}";
                }
            }

        }

        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Checks if hash is supported.
        /// </summary>
        /// <param name="hashString">The hash.</param>
        /// <returns>Is supported?</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("HASH|V1$");
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hashedPassword">The hash.</param>
        /// <returns>Could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // Check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$HASH|V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Get result
                for (var i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        public static string BValidRegistr(string FirstName, string LastName, string Email, string password1, string password2, string login)
        {
            Regex r = new Regex("^[a-zA-Z]{3,50}$");

            if (!r.IsMatch(FirstName))
            {
                return "First name should not contain spaces and numbers. It should contains 3 to 500 characters";
            }

            r = new Regex("^[2\\p{L}\\-\\s]{3,50}$");
            if (!r.IsMatch(LastName))
            {
                return "Wrong last name format. There should be 3 to 500 characters space or dash.";
            }
            r = new Regex("^[a-zA-Z0-9]{3,50}$");
            if (!r.IsMatch(login))
            {
                return "Login should contain at least 3 characters or numbers.";
            }
            r = new Regex(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$");

            if (!r.IsMatch(Email))
            {
                return "Email address not valid";
            }

            if (!password1.Equals(password2))
            {
                return "Paswords differs.";
            }
            if(password1.Length <3 || password1.Length > 20)
            {
                return "Password should contain 3 to 20 characters";
            }

            return string.Empty;
        }

        public static string CheckIfUserExist(User user)
        {
            List<User> users;
            try
            {
                users = GlobalSettings.Connection.UsersGetAll();
            }
            catch(Exception)
            {
                return "We have problem with database connection.";
            }
            if (users.Exists(x => x.Email.Equals(user.Email) ))
            {
                return "Email already exists";
            }
            if (users.Exists(x => x.LoginStr.Equals(user.LoginStr)))
            {
                return "Login already exists";
            }

            return string.Empty;
        }

        public static string IsLoginValid(string login, string password)
        {
            Regex r = new Regex("^[a-zA-Z]{3,50}$");
            r = new Regex("^[a-zA-Z0-9]{3,50}$");
            if (!r.IsMatch(login))
            {
                return "Login should contain at least 3 characters or numbers.";
            }
            if (password.Length < 3 || password.Length > 20)
            {
                return "Password should contain 3 to 20 characters";
            }
            return string.Empty;

        }
    }
}
