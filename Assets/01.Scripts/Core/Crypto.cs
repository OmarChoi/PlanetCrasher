using System;
using System.Security.Cryptography;

public static class Crypto
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;
    
    public static string ConvertPasswordToHash(string password)
    {
        byte[] salt = GenerateSalt();
        byte[] hash = HashPasswordWithSalt(password, salt, Iterations);
        return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }
    
    public static bool VerifyPassword(string password, string storedHash)
    {
        try
        {
            string[] parts = storedHash.Split(':');
            if (parts.Length != 3) return false;
                
            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] originalHash = Convert.FromBase64String(parts[2]);
            
            byte[] testHash = HashPasswordWithSalt(password, salt, iterations);
            
            return CompareHashes(originalHash, testHash);
        }
        catch
        {
            return false;
        }
    }
    
    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
    
    private static byte[] HashPasswordWithSalt(string password, byte[] salt, int iterations)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
        {
            return pbkdf2.GetBytes(HashSize);
        }
    }
    
    private static bool CompareHashes(byte[] hash1, byte[] hash2)
    {
        return CryptographicOperations.FixedTimeEquals(hash1, hash2);
    }
}
