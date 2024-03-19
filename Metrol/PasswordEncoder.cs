using System.Security.Cryptography;

namespace Metrol
{
    /// <summary>
    /// Кодировщик пароля.
    /// </summary>
    public static class PasswordEncoder
    {
        /// <summary>
        /// Генерация хэша пароля.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <param name="salt">Соль.</param>
        /// <returns>Хэш пароля.</returns>
        public static string GenerateSaltedHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Передан пустой пароль.");
            }

            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException(nameof(password), "Передана пустая соль.");
            }

            HashAlgorithm algorithm = new SHA256Managed();

            byte[] passwordBytes = System.Text.Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] plainTextWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
            saltBytes.CopyTo(plainTextWithSaltBytes, 0);
            passwordBytes.CopyTo(plainTextWithSaltBytes, saltBytes.Length);

            byte[] hash = algorithm.ComputeHash(plainTextWithSaltBytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
