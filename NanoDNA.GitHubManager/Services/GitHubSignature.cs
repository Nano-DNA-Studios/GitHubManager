using System;
using System.Text;
using System.Security.Cryptography;

namespace NanoDNA.GitHubManager.Services
{
    /// <summary>
    /// Encapsulates the logic to verify the GitHub signature.
    /// </summary>
    internal class GitHubSignature
    {
        /// <summary>
        /// Verifies if the Signature is Valid and comes from GitHub.
        /// </summary>
        /// <param name="body">Body of the Signature</param>
        /// <param name="signatureHeader">Header of the Signature</param>
        /// <param name="secret">Secret from the HttpRequest</param>
        /// <returns>True if the Signature is Valid, False otherwise</returns>
        public static bool Verify(string body, string signatureHeader, string secret)
        {
            if (string.IsNullOrEmpty(signatureHeader)) return false;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            var hashString = "sha256=" + BitConverter.ToString(hash).Replace("-", "").ToLower();
            return hashString == signatureHeader;
        }
    }
}