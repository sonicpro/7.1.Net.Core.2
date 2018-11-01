using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Packt.CS7
{
	public class Protector
	{
		// Prepare salt and iteration count for IV and Key byte arrays generator.
		private static readonly byte[] salt = Encoding.Unicode.GetBytes("7BANANAS"); // 16-byte long salt.
		private static readonly int iterations = 2000;

		// Encrypt symmetrically.
		public static string Encrypt(string plainText, string password)
		{
			byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);
			var aes = Aes.Create();

			// Create "byte array generator"
			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
			aes.Key = pbkdf2.GetBytes(32); // 256 (32*8)-bit key.
			aes.IV = pbkdf2.GetBytes(16);  // 128-bit IV
			var ms = new MemoryStream();
			using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cs.Write(plainBytes, 0, plainBytes.Length);
			}
			return Convert.ToBase64String(ms.ToArray());
		}

		public static string Decrypt(string cryptoText, string password)
		{
			byte[] cryptoBytes = Convert.FromBase64String(cryptoText);
			var aes = Aes.Create();
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
			aes.Key = pbkdf2.GetBytes(32);
			aes.IV = pbkdf2.GetBytes(16);
			var ms = new MemoryStream();
			using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cs.Write(cryptoBytes, 0, cryptoBytes.Length);
			}
			return Encoding.Unicode.GetString(ms.ToArray());
		}

	}
}
