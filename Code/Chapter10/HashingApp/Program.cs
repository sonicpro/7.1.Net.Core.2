using Packt.CS7;
using static System.Console;

namespace HashingApp
{
	class Program
	{
		static void Main(string[] args)
		{
			WriteLine("A user named Alice has been registered with Pa$$w0rd as her password.");

			var alice = Protector.Register("Alice", "Pa$$w0rd");

			WriteLine($"Name: {alice.Name}");
			WriteLine($"Salt: {alice.Salt}");
			WriteLine($"Salted and hashed password: {alice.SaltedHashedPassword}");

			WriteLine();

			Write("Enter a different username to register: ");
			string userName = ReadLine();
			Write("Enter a password to register: ");
			string password = ReadLine();
			var user = Protector.Register(userName, password);

			WriteLine($"Name: {user.Name}");
			WriteLine($"Salt: {user.Salt}");
			WriteLine($"Salted and hashed password: {user.SaltedHashedPassword}");

			bool correctPassword = false;
			while (!correctPassword)
			{
				Write("Enter a user name to log in: ");
				string loginUserName = ReadLine();
				Write("Enter a password to log in: ");
				string loginPassword = ReadLine();
				correctPassword = Protector.CheckPassword(loginUserName, loginPassword);
				if (correctPassword)
				{
					WriteLine($"Correct! {loginUserName} has been logged in.");
				}
				else
				{
					WriteLine("Invalid user name or password. Try again.");
				}
			}
		}
	}
}
