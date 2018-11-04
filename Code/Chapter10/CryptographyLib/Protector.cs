using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Packt.CS7
{
	public class Protector
	{
		// Prepare salt and iteration count for IV and Key byte arrays generator.
		private static readonly byte[] salt = Encoding.Unicode.GetBytes("7BANANAS"); // 16-byte long salt.
		private static readonly int iterations = 2000;
		// Store salts, hashed passwords keyed by User name.
		private static Dictionary<string, User> Users = new Dictionary<string, User>();

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

		public static User Register(string userName, string password, string[] roles = null)
		{
			// generate a random salt
			var rng = RandomNumberGenerator.Create();
			var saltBytes = new byte[16];
			rng.GetBytes(saltBytes);
			var saltText = Convert.ToBase64String(saltBytes);

			// generate the salted and hashed password
			var sha = SHA256.Create();
			var salted = password + saltText;
			var saltedHashedPassword = Convert.ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(salted)));

			var user = new User
			{
				Name = userName,
				Salt = saltText,
				SaltedHashedPassword = saltedHashedPassword,
				Roles = roles
			};
			Users.Add(userName, user);

			return user;
		}

		public static bool CheckPassword(string userName, string password)
		{
			if (!Users.ContainsKey(userName))
			{
				return false;
			}

			var user = Users[userName];

			// Regenerate the hash.
			var sha = SHA256.Create();
			var salted = password + user.Salt;
			var saltedHashedPassword = Convert.ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(salted)));

			return saltedHashedPassword == user.SaltedHashedPassword;
		}

		public static void RegisterSomeUsers()
		{
			Register("Alice", "Pa$$w0rd", new[] { "Admins" });
			Register("Bob", "Pa$$w0rd", new[] { "Sales", "TeamLeads" });
			Register("Eve", "Pa$$w0rd");
		}

		// Login a user and assign its principal to the current thread.
		public static void LogIn(string userName, string password)
		{
			if (CheckPassword(userName, password))
			{
				var identity = new GenericIdentity(userName, "PacktAuthType");
				var principal = new GenericPrincipal(identity, Users[userName].Roles);
				System.Threading.Thread.CurrentPrincipal = principal;
			}
		}
	}
}
