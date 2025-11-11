using System;
using System.Security.Cryptography;

namespace bluesky.Services.Security
{
    /// <summary>
    /// Hasher PBKDF2 compatible con .NET Framework.
    /// Guarda como formato: iterations:salt:hash
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static string HashPassword(string plain)
        {
            if (string.IsNullOrWhiteSpace(plain))
                throw new ArgumentException("Password vacío", nameof(plain));

            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);

                var hash = Pbkdf2(plain, salt, Iterations, HashSize);
                return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

        public static bool VerifyPassword(string plain, string stored)
        {
            if (string.IsNullOrWhiteSpace(plain) || string.IsNullOrWhiteSpace(stored))
                return false;

            var parts = stored.Split(':');
            if (parts.Length != 3) return false;

            int iters;
            if (!int.TryParse(parts[0], out iters)) return false;
            var salt = Convert.FromBase64String(parts[1]);
            var hash = Convert.FromBase64String(parts[2]);

            var test = Pbkdf2(plain, salt, iters, hash.Length);
            return FixedTimeEquals(hash, test);
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int length)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(length);
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
