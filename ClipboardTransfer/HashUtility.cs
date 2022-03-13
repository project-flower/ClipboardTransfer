using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ClipboardTransfer
{
    internal static class HashUtility
    {
        public static string HashFromFile(string fileName)
        {
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    return HashFromStream(stream);
                }
            }
            catch (Exception exception)
            {
                return $"({exception.Message})";
            }
        }

        public static string HashFromStream(Stream stream)
        {
            try
            {
                var provider = new MD5CryptoServiceProvider();

                try
                {
                    byte[] hash = provider.ComputeHash(stream);
                    return string.Join(string.Empty, hash.Select(n => n.ToString("x2")));
                }
                finally
                {
                    provider.Clear();
                }
            }
            catch (Exception exception)
            {
                return $"({exception.Message})";
            }
        }
    }
}
